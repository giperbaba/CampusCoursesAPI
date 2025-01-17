using Microsoft.EntityFrameworkCore;
using repassAPI.Constants;
using repassAPI.Data;
using repassAPI.Entities;
using repassAPI.Exceptions;
using repassAPI.Models.Enums;
using repassAPI.Models.Request;
using repassAPI.Models.Response;
using repassAPI.Services.Interfaces;
using repassAPI.Utils;

namespace repassAPI.Services.Impl;

public class CourseService: ICourseService
{
    private readonly DatabaseContext _context;

    public CourseService(DatabaseContext context)
    {
        _context = context;
    }

    //admin
    public async Task<CoursePreviewResponse> CreateCourse(string groupId, CourseCreateRequest courseCreate)
    {
        var course = Mapper.MapCourseFromCreateModelToEntity(Guid.Parse(groupId), courseCreate, courseCreate.MaxStudentsCount, CourseStatus.Created, courseCreate.MainTeacherId);
        
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
        
        await AddTeacher(course.Id, courseCreate.MainTeacherId, true);
        
        var courseResponse = Mapper.MapCourseEntityToCoursePreviewModel(course);
        return courseResponse;
    }

    public async Task<CourseDetailedResponse> EditCourse(string courseId, CourseEditRequest request)
    {
        var course = GetCourseById(Guid.Parse(courseId));

        if (course.Students.Count > request.MaxStudentsCount)
        {
            throw new ConflictException(ErrorMessages.ConflictStudentsCount);
        }
        Edit(course, request);
        await _context.SaveChangesAsync();
        
        return await GetCourseDetailedInfo(courseId);
    }
    
    public async Task<CoursePreviewResponse> DeleteCourse(string id)
    {
        var course = GetCourseById(Guid.Parse(id));
        
        var courseTeachers = _context.CourseTeachers.Where(ct => ct.CourseId == course.Id).ToList();
        _context.CourseTeachers.RemoveRange(courseTeachers);
        
        CheckTeachersRole(course, courseTeachers);

        var courseStudents = _context.CourseStudents.Where(cs => cs.CourseId == course.Id).ToList();
        _context.CourseStudents.RemoveRange(courseStudents);
        
        CheckStudentsRole(course, courseStudents);

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();

        var coursePreview = Mapper.MapCourseEntityToCoursePreviewModel(course);
        return coursePreview;
    }

    //admin, main teacher
    public async Task<CourseDetailedResponse> AddTeacherToCourse(string courseId, CourseAddTeacherRequest request)
    {
        await AddTeacher(Guid.Parse(courseId), Guid.Parse(request.userId.ToString()), false);
        return await GetCourseDetailedInfo(courseId);
    }
    
    
    //admin, teacher
    public async Task<CourseDetailedResponse> EditCourseStatus(string courseId, CourseEditStatusRequest status)
    {
        var course = GetCourseById(Guid.Parse(courseId));

        if ((int)status.status < (int)course.Status)
        {
            throw new BadRequestException(ErrorMessages.InvalidCourseStatus);
        }
        
        course.Status = status.status;
        _context.Courses.Update(course); 
        await _context.SaveChangesAsync();
        
        return await GetCourseDetailedInfo(courseId);
    }

    public async Task<CourseDetailedResponse> EditCourseReqAndAnnotations(string courseId,
        CourseEditReqAndAnnotationsRequest request)
    {
        var course = GetCourseById(Guid.Parse(courseId));
        
        course.Requirements = request.Requirements;
        course.Annotations = request.Annotations;
        _context.Courses.Update(course); 
        await _context.SaveChangesAsync();

        return await GetCourseDetailedInfo(courseId);
    }
    
    //anybody
    public async Task<IResult> SignUp(string courseId, string userId)
    {
        var course = GetCourseById(Guid.Parse(courseId));
        if (course.Status != CourseStatus.OpenForAssigning)
        {
            throw new BadRequestException(ErrorMessages.InvalidCourseStatusInRequest);
        }
        await AddStudent(Guid.Parse(courseId), Guid.Parse(userId));
        return Results.Ok();
    }
    
    public async Task<CourseDetailedResponse> GetCourseDetailedInfo(string courseId)
    {
        var course = GetCourseById(Guid.Parse(courseId));
        
        IList<CourseStudentResponse> students = [];
        IList<CourseTeacherResponse> teachers = [];
        IList<CourseNotificationResponse> notifications = [];

        foreach (CourseStudent studentEntity in course.Students)
        {
            students.Add(Mapper.MapStudentEntityToStudentModel(studentEntity));
        }

        foreach (CourseTeacher teacherEntity in course.Teachers)
        {
            teachers.Add(Mapper.MapTeacherEntityToTeacherModel(teacherEntity));
        }

        foreach (Notification notificationEntity in course.Notifications)
        {
            notifications.Add(Mapper.MapNotificationEntityToNotificationModel(notificationEntity));
        }

        var courseDetailedModel = Mapper.MapCourseEntityToCourseDetailsModel(course, students, teachers, notifications);
        return courseDetailedModel;
    }
    
    //check
    public bool IsUserMainTeacher(string courseId, string userId)
    {
        var course = GetCourseById(Guid.Parse(courseId));
        return course.MainTeacherId == Guid.Parse(userId);
    }

    public bool IsUserTeacher(string courseId, string userId)
    {
        var course = GetCourseById(Guid.Parse(courseId));
        return course.Teachers.Any(teacher => teacher.TeacherId.ToString() == userId);
    }
    
    //private
    private Course GetCourseById(Guid courseId)
    {
        var course = _context.Courses
            .Include(c => c.Teachers) 
            .ThenInclude(ct => ct.Teacher)
            .Include(c => c.Students) 
            .ThenInclude(ct => ct.Student)
            .Include(c => c.Notifications) 
            .FirstOrDefault(c => c.Id == courseId);
    
        if (course == null)
        {
            throw new NotFoundException(ErrorMessages.CourseNotFound);
        }
        return course;
    }
    private async Task AddTeacher(Guid courseId, Guid teacherId, bool isMainTeacher)
    {
        await CheckIsCourseExist(courseId);
        await CheckIsUserExist(teacherId);
        
        await CheckIsAlreadyStudent(teacherId, courseId);
        await CheckIsAlreadyTeacher(teacherId, courseId);
        
        await _context.CourseTeachers.AddAsync(new CourseTeacher(courseId, teacherId, isMainTeacher));
        
        await GetTeacherRole(teacherId);

        await _context.SaveChangesAsync();
    }

    
    private async Task AddStudent(Guid courseId, Guid studentId, StudentStatus studentStatus = StudentStatus.InQueue)
    {
        await CheckIsCourseExist(courseId);
        await CheckIsUserExist(studentId);

        await CheckIsAlreadyStudent(studentId, courseId);
        await CheckIsAlreadyTeacher(studentId, courseId);
        
        
        var course = GetCourseById(courseId);
        if (course.RemainingSlotsCount > 0)
        {
            await _context.CourseStudents.AddAsync(new CourseStudent(courseId, studentId));
            
            await GetStudentRole(studentId);
            course.RemainingSlotsCount -= 1;

            await _context.SaveChangesAsync();
        }
        else
        {
            throw new ConflictException(ErrorMessages.ConflictCourseRemainingSlots);
        }
    }

    private async Task CheckIsUserExist(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
            throw new NotFoundException(ErrorMessages.UserNotFound);
    }

    private async Task CheckIsCourseExist(Guid id)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == id);
        if (course == null)
            throw new NotFoundException(ErrorMessages.CourseNotFound);
    }

    private async Task CheckIsAlreadyStudent(Guid userId, Guid courseId)
    {
        var isAlreadyStudent = await _context.CourseStudents
            .AnyAsync(cs => cs.CourseId == courseId && cs.StudentId == userId);
        if (isAlreadyStudent)
            throw new ConflictException(ErrorMessages.AlreadyIsStudent);
    }

    private async Task CheckIsAlreadyTeacher(Guid userId, Guid courseId)
    {
        var isAlreadyTeacher = await _context.CourseTeachers
            .AnyAsync(ct => ct.CourseId == courseId && ct.TeacherId == userId);
        if (isAlreadyTeacher)
            throw new ConflictException(ErrorMessages.AlreadyIsTeacher);
    }

    private async Task GetTeacherRole(Guid userId)
    {
        var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId);
        if (userRole != null)
        {
            userRole.IsTeacher = true;
            _context.UserRoles.Update(userRole);
        }
    }

    private async Task GetStudentRole(Guid userId)
    {
        var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId);
        if (userRole != null)
        {
            userRole.IsStudent = true;
            _context.UserRoles.Update(userRole);
        }
    }

    private async Task<string> GetNameById(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        
        if (user == null)
            throw new NotFoundException(ErrorMessages.CourseNotFound);
        
        return user.FullName;
    }

    private async Task<string> GetEmailById(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        
        if (user == null)
            throw new NotFoundException(ErrorMessages.CourseNotFound);
        
        return user.Email;
    }

    private void CheckTeachersRole(Course course, List<CourseTeacher> courseTeachers)
    {
        foreach (var teacher in courseTeachers)
        {
            var isTeacherInOtherCourses = _context.CourseTeachers.Any(ct => ct.TeacherId == teacher.TeacherId && ct.CourseId != course.Id);
            if (!isTeacherInOtherCourses)
            {
                var userRole = _context.UserRoles.FirstOrDefault(ur => ur.UserId == teacher.TeacherId);
                if (userRole != null)
                {
                    userRole.IsTeacher = false;
                    _context.UserRoles.Update(userRole);
                }
            }
        }
    }
    
    private void CheckStudentsRole(Course course, List<CourseStudent> courseStudents)
    {
        foreach (var student in courseStudents)
        {
            var isStudentInOtherCourses = _context.CourseStudents.Any(cs => cs.StudentId == student.StudentId && cs.CourseId != course.Id);
            if (!isStudentInOtherCourses)
            {
                var userRole = _context.UserRoles.FirstOrDefault(ur => ur.UserId == student.StudentId);
                if (userRole != null)
                {
                    userRole.IsStudent = false;
                    _context.UserRoles.Update(userRole);
                }
            }
        }
    }
    
    private void Edit(Course course, CourseEditRequest request)
    {
        course.Name = request.Name;
        course.StartYear = request.StartYear;
        course.RemainingSlotsCount = request.MaxStudentsCount - (course.MaxStudentsCount - course.RemainingSlotsCount);
        course.MaxStudentsCount = request.MaxStudentsCount;
        course.Semester = request.Semester;
        course.Requirements = request.Requirements;
        course.Annotations = request.Annotations;
    }
}
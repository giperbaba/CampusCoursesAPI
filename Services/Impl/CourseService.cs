using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    private readonly IAccountService _accountService;

    public CourseService(DatabaseContext context, IAccountService accountService)
    {
        _context = context;
        _accountService = accountService;
    }

    //admin
    public async Task<CoursePreviewResponse> CreateCourse(string groupId, CourseCreateRequest courseCreate)
    {
        await CheckIsUserExist(courseCreate.MainTeacherId);

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
        
        return await GetCourseDetailedInfo(courseId, null);
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
        return await GetCourseDetailedInfo(courseId, null);
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
        
        return await GetCourseDetailedInfo(courseId, null);
    }

    public async Task<CourseDetailedResponse> EditCourseReqAndAnnotations(string courseId,
        CourseEditReqAndAnnotationsRequest request)
    {
        var course = GetCourseById(Guid.Parse(courseId));
        
        course.Requirements = request.Requirements;
        course.Annotations = request.Annotations;
        _context.Courses.Update(course); 
        await _context.SaveChangesAsync();

        return await GetCourseDetailedInfo(courseId, null);
    }
    
    public async Task<CourseDetailedResponse> CreateNewNotification(string courseId,
        NotificationCreateRequest notificationCreateRequest)
    {
        var course = GetCourseById(Guid.Parse(courseId));

        if (course == null)
        {
            throw new NotFoundException(ErrorMessages.CourseNotFound);
        }

        var notification = Mapper.MapNotificationCreateModelToEntity(courseId, notificationCreateRequest);

        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();

        return await GetCourseDetailedInfo(courseId, null);
    }



    public async Task<CourseDetailedResponse> EditStudentStatus(string courseId, string studentId,
        CourseStudentEditStatusRequest editStatusRequest)
    {
        await CheckIsUserExist(Guid.Parse(studentId));
    
        var course = GetCourseById(Guid.Parse(courseId));

        if (course.MaxStudentsCount == course.StudentsEnrolledCount && editStatusRequest.Status == StudentStatus.Accepted)
        {
            throw new ConflictException(ErrorMessages.ConflictCourseRemainingSlots);
        }
        
        await CheckIsStudentInQueue(studentId, courseId);
        
        var student = course.Students.FirstOrDefault(s => s.StudentId.ToString().ToLower() == studentId.ToLower());
    
        if (student == null)
        {
            throw new NotFoundException(ErrorMessages.ConflictStudentIsNotInTheCourse);
        }
        
        student.Status = editStatusRequest.Status;
        student.MidtermResult = StudentMark.NotDefined;
        student.FinalResult = StudentMark.NotDefined;
        
        _context.CourseStudents.Update(student);
        await _context.SaveChangesAsync();

        return await GetCourseDetailedInfo(courseId, null!);
    }

    
    
    public async Task<CourseDetailedResponse> EditStudentMark(string courseId, string studentId,
        CourseStudentEditMarkRequest editMarkRequest)
    {
        await CheckIsUserExist(Guid.Parse(studentId));
    
        var course = GetCourseById(Guid.Parse(courseId));
        
        var student = course.Students.FirstOrDefault(s => s.StudentId.ToString().ToLower() == studentId.ToLower());
        if (student == null)
        {
            throw new NotFoundException(ErrorMessages.ConflictStudentIsNotInTheCourse);
        }

        if (editMarkRequest.MarkType == MarkType.Final)
        {
            student.FinalResult = editMarkRequest.Mark;
        }
        
        else if (editMarkRequest.MarkType == MarkType.Midterm)
        {
            student.MidtermResult = editMarkRequest.Mark;
        }
    
        _context.CourseStudents.Update(student);
        await _context.SaveChangesAsync();

        return await GetCourseDetailedInfo(courseId, null!);
    }

    
    //anybody
    public async Task<IResult> SignUp(string courseId, string userId)
    {
        var course = GetCourseById(Guid.Parse(courseId));
        if (course.Status != CourseStatus.OpenForAssigning)
        {
            throw new BadRequestException(ErrorMessages.InvalidCourseStatusInRequest);
        }
        await AddStudentToQueue(Guid.Parse(courseId), Guid.Parse(userId));
        return Results.Ok();
    }

    public async Task<IList<CoursePreviewResponse>> GetStudingCourses(string userId)
    {
        var user = await _context.Users
            .Include(u => u.StudingCourses)
            .ThenInclude(sc => sc.Course) 
            .FirstOrDefaultAsync(u => u.Id.ToString() == userId);
        var courses = user.StudingCourses;

        IList<CoursePreviewResponse> coursesResponse = [];
        foreach (CourseStudent courseStudent in courses)
        {
            coursesResponse.Add(Mapper.MapCourseEntityToCoursePreviewModel(courseStudent.Course));
        }

        return coursesResponse;
    }

    public async Task<IList<CoursePreviewResponse>> GetMyTeachingCourses(string userId)
    {
        var user = await _context.Users
            .Include(u => u.TeachingCourses) 
            .ThenInclude(tc => tc.Course) 
            .FirstOrDefaultAsync(u => u.Id.ToString() == userId);
        
        var courses = user.TeachingCourses;

        IList<CoursePreviewResponse> coursesResponse = [];
        foreach (CourseTeacher courseTeacher in courses)
        {
            coursesResponse.Add(Mapper.MapCourseEntityToCoursePreviewModel(courseTeacher.Course));
        }

        return coursesResponse;
    }
    
    public async Task<CourseDetailedResponse> GetCourseDetailedInfo(string courseId, string? userId)
    {
        var course = GetCourseById(Guid.Parse(courseId));
    
        IList<CourseStudentResponse> students = GetStudentsList(course, userId);
        IList<CourseTeacherResponse> teachers = GetTeachersList(course);
        IList<CourseNotificationResponse> notifications = GetNotificationsList(course);

        var courseDetailedModel = Mapper.MapCourseEntityToCourseDetailsModel(course, students, teachers, notifications);
        return courseDetailedModel;
    }
    
    public async Task<IEnumerable<CoursePreviewResponse>> GetCourses(SortType? sort, string? search, 
        bool? hasPlacesAndOpen, Semester? semester, int page, int pageSize)
    {
        if (page <= 0 || pageSize <= 0)
        {
            throw new BadRequestException(ErrorMessages.InvalidPageCountOrPageSize);
        }
        
        var query = _context.Courses
            .Include(c => c.Students)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(course => course.Name.Contains(search));
        }

        if (hasPlacesAndOpen == true)
        {
            query = query.Where(course => course.RemainingSlotsCount > 0 && course.Status == CourseStatus.OpenForAssigning);
        }

        if (semester.HasValue)
        {
            query = query.Where(course => course.Semester == semester.Value);
        }
    
        query = sort switch
        {
            SortType.CreatedAsc => query.OrderBy(course => course.CreateTime),
            SortType.CreatedDesc => query.OrderByDescending(course => course.CreateTime),
            _ => query.OrderBy(course => course.CreateTime) 
        };
    
        query = query.Skip((page - 1) * pageSize).Take(pageSize);
    
        return await query
            .Select(course => Mapper.MapCourseEntityToCoursePreviewModel(course))
            .ToListAsync();
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

    
    private async Task AddStudentToQueue(Guid courseId, Guid studentId, StudentStatus studentStatus = StudentStatus.InQueue)
    {
        await CheckIsCourseExist(courseId);
        await CheckIsUserExist(studentId);

        await CheckIsAlreadyStudent(studentId, courseId);
        await CheckIsAlreadyTeacher(studentId, courseId);
        
        await _context.CourseStudents.AddAsync(new CourseStudent(courseId, studentId));
            
        await GetStudentRole(studentId);

        await _context.SaveChangesAsync();
    }

    private async Task CheckIsUserExist(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString().ToLower() == id.ToString().ToLower());
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
            throw new BadRequestException(ErrorMessages.AlreadyIsStudent);
    }

    private async Task CheckIsAlreadyTeacher(Guid userId, Guid courseId)
    {
        var isAlreadyTeacher = await _context.CourseTeachers
            .AnyAsync(ct => ct.CourseId == courseId && ct.TeacherId == userId);
        if (isAlreadyTeacher)
            throw new BadRequestException(ErrorMessages.AlreadyIsTeacher);
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
    
    private IList<CourseStudentResponse> GetStudentsList(Course course, string? userId)
    {
        IList<CourseStudentResponse> students = new List<CourseStudentResponse>();

        foreach (CourseStudent studentEntity in course.Students)
        {
            // если админ учитель или этот ученик то показываем резы
            if (userId == null || 
                studentEntity.Id.ToString() == userId || 
                _accountService.IsUserAdmin(userId) || 
                IsUserTeacher(course.Id.ToString(), userId))
            {
                students.Add(Mapper.MapStudentEntityToStudentModelWithResults(studentEntity));
            }
            // в других случ не показываем
            else if (studentEntity.Status == StudentStatus.Accepted)
            {
                students.Add(Mapper.MapStudentEntityToStudentModelWithoutResults(studentEntity));
            }
        }

        return students;
    }
    
    private IList<CourseTeacherResponse> GetTeachersList(Course course)
    {
        IList<CourseTeacherResponse> teachers = [];
        foreach (CourseTeacher teacherEntity in course.Teachers)
        {
            teachers.Add(Mapper.MapTeacherEntityToTeacherModel(teacherEntity));
        }

        return teachers;
    }
    
    private IList<CourseNotificationResponse> GetNotificationsList(Course course)
    {
        IList<CourseNotificationResponse> notifications = [];

        foreach (Notification notificationEntity in course.Notifications)
        {
            notifications.Add(Mapper.MapNotificationEntityToNotificationModel(notificationEntity));
        }

        return notifications;
    }

    private async Task CheckIsStudentInQueue(string studentId, string courseId)
    {
        var isStudentInQueue = await _context.CourseStudents
            .AnyAsync(cs => cs.StudentId.ToString() == studentId && cs.CourseId.ToString() == courseId && cs.Status == StudentStatus.InQueue);

        if (!isStudentInQueue)
        {
            throw new BadRequestException(ErrorMessages.StudentNotInQueue);
        }
    }
}

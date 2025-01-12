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

    public async Task<CoursePreviewResponse> CreateCourse(string groupId, CourseCreateRequest courseCreate)
    {
        var course = Mapper.MapCourseFromCreateModelToEntity(Guid.Parse(groupId), courseCreate, courseCreate.MaxStudentsCount, CourseStatus.Created);
        
        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();
        
        await AddTeacher(course.Id, courseCreate.MainTeacherId, true);
        
        var courseResponse = Mapper.MapCourseEntityToCourseModel(course);
        return courseResponse;
    }

    private async Task AddTeacher(Guid courseId, Guid teacherId, bool isMainTeacher)
    {
        await CheckIsCourseExist(courseId);
        await CheckIsUserExist(teacherId);
        
        await CheckIsAlreadyStudent(teacherId, courseId);
        await CheckIsAlreadyTeacher(teacherId, courseId);
        
        await _context.CourseTeachers.AddAsync(new CourseTeacher(courseId, teacherId, isMainTeacher));
        await _context.SaveChangesAsync();
    }
    
    private async Task AddStudent(Guid courseId, Guid studentId, StudentStatus studentStatus = StudentStatus.InQueue)
    {
        await CheckIsCourseExist(courseId);
        await CheckIsUserExist(studentId);

        await CheckIsAlreadyStudent(studentId, courseId);
        await CheckIsAlreadyTeacher(studentId, courseId);
        
        await _context.CourseStudents.AddAsync(new CourseStudent(courseId, studentId, studentStatus));
        await _context.SaveChangesAsync();
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
}
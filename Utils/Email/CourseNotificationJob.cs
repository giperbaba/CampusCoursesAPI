using Microsoft.EntityFrameworkCore;
using Quartz;
using repassAPI.Data;
using repassAPI.Models.Enums;
using repassAPI.Services.Interfaces;

namespace repassAPI.Utils.Email;

public class CourseNotificationJob : IJob
{
    private readonly DatabaseContext _dbContext;
    private readonly IEmailService _emailSender;

    public CourseNotificationJob(DatabaseContext dbContext, IEmailService emailSender)
    {
        _dbContext = dbContext;
        _emailSender = emailSender;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var currentDate = DateTime.UtcNow.Date;
        
        var upcomingCourses = await _dbContext.Courses
            .Include(c => c.Students)
            .ThenInclude(cs => cs.Student)
            .Where(c =>
                (c.Semester == Semester.Spring && currentDate == new DateTime(currentDate.Year, 2, 28)) || 
                (c.Semester == Semester.Autumn && currentDate == new DateTime(currentDate.Year, 8, 31))) 
            .ToListAsync();

        foreach (var course in upcomingCourses)
        {
            var acceptedStudents = course.Students
                .Where(cs => cs.Status == StudentStatus.Accepted)
                .Select(cs => cs.Student)
                .ToList();

            foreach (var student in acceptedStudents)
            {
                var subject = $"Напоминание о начале курса \"{course.Name}\"";
                var body = $" Уважаемый(ая) {student.FullName}, напоминаем, что курс {course.Name} начинается завтра";

                await _emailSender.SendEmail(student.Email, subject, body);
            }
        }
    }
}
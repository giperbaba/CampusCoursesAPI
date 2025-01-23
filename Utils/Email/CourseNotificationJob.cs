using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;
using Quartz;
using repassAPI.Data;
using repassAPI.Models.Enums;
using repassAPI.Services.Interfaces;

namespace repassAPI.Utils.Email;

public class CourseNotificationJob : IJob
{
    private readonly DatabaseContext _dbContext;
    private readonly IEmailService _emailService;
    private readonly AsyncRetryPolicy _retryPolicy;

    public CourseNotificationJob(DatabaseContext dbContext, IEmailService emailService)
    {
        _dbContext = dbContext;
        _emailService = emailService;
        
        _retryPolicy = Policy
            .Handle<HttpRequestException>() 
            .WaitAndRetryAsync(
                retryCount: 3, 
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    Console.WriteLine($"Retry {retryCount} after {timeSpan.TotalSeconds} seconds by reason of: {exception.Message}");
                });
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var currentDate = DateTime.UtcNow.Date;
        
        var upcomingCourses = await _dbContext.Courses
            .Include(c => c.Students)
            .ThenInclude(cs => cs.Student)
            .Where(c =>
                (c.Semester == Semester.Spring && currentDate == new DateTime(currentDate.Year, 2, 28)) || 
                (c.Semester == Semester.Autumn && currentDate == DateTime.UtcNow.Date)) 
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

                await _retryPolicy.ExecuteAsync(async () =>
                {
                    await _emailService.SendEmail(student.Email, subject, body);
                });
            }
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;
using Polly.CircuitBreaker;
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
    private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

    public CourseNotificationJob(DatabaseContext dbContext, IEmailService emailService)
    {
        _dbContext = dbContext;
        _emailService = emailService;

        _retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(
                retryCount: 5,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (exception, timeSpan, retryCount) =>
                {
                    Console.WriteLine($"Попытка {retryCount} через {timeSpan.TotalSeconds} секунд, ошибка: {exception.Message}");
                });

        _circuitBreakerPolicy = Policy
            .Handle<HttpRequestException>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromMinutes(5),
                onBreak: (exception, duration) =>
                {
                    Console.WriteLine($"Блокировка запросов на {duration.TotalSeconds} секунд, ошибка: {exception.Message}");
                },
                onReset: () =>
                {
                    Console.WriteLine("Запросы снова разрешены");
                },
                onHalfOpen: () =>
                {
                    Console.WriteLine("Тестирование соединения");
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
                var body = $"Уважаемый(ая) {student.FullName}, напоминаем, что курс {course.Name} начинается завтра";

                try
                {
                    await _circuitBreakerPolicy.ExecuteAsync(async () =>
                    {
                        await _retryPolicy.ExecuteAsync(async () =>
                        {
                            await _emailService.SendEmail(student.Email, subject, body);
                        });
                    });
                }
                catch (BrokenCircuitException)
                {
                    Console.WriteLine($"Письмо на {student.Email} не отправлено");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при отправке письма на {student.Email}: {ex.Message}");
                }
            }
        }
    }
}
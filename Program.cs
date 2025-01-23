using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using repassAPI.Constants;
using repassAPI.Data;
using repassAPI.Entities;
using repassAPI.Middleware;
using repassAPI.Services.Impl;
using repassAPI.Services.Interfaces;
using repassAPI.Utils;
using repassAPI.Utils.Email;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Type = SecuritySchemeType.Http,
        In = ParameterLocation.Header,
        Description = "Please enter token"
    });

    options.OperationFilter<CustomSecurityRequirementsFilter>();
});

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection")));

builder.Services.AddQuartz(q =>
{
    var springJobKey = new JobKey("SpringCourseNotificationJob");
    q.AddJob<CourseNotificationJob>(opts => opts.WithIdentity(springJobKey));
    q.AddTrigger(opts => opts
        .ForJob(springJobKey)
        .WithIdentity("SpringCourseNotification-trigger")
        .WithCronSchedule("0 0 12 28 2 ?")); //28 февраля 12:00 / каждую секунду "0/1 * * * * ?" 

    var autumnJobKey = new JobKey("AutumnCourseNotificationJob");
    q.AddJob<CourseNotificationJob>(opts => opts.WithIdentity(autumnJobKey));
    q.AddTrigger(opts => opts
        .ForJob(autumnJobKey)
        .WithIdentity("AutumnCourseNotification-trigger")
        .WithCronSchedule("0/15 * * * * ?")); //31 августа 12:00
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddScoped<Tokens>();
builder.Services.AddScoped<TokenProvider>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            builder.Configuration.GetSection("AppSettings:AccessKey").Value!))
    };
});

var app = builder.Build();

//seeding 
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    db.Database.Migrate();

    if (!db.Users.Any())
    {
        var usersJson = File.ReadAllText("users.json");
        var users = JsonSerializer.Deserialize<List<User>>(usersJson);

        if (users != null)
        {
            foreach (var user in users)
            {
                user.Id = Guid.NewGuid();
                db.Users.Add(user);
            }
            db.SaveChanges();
        }
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization(); 

app.MapControllers();

app.Run();
using Microsoft.EntityFrameworkCore;
using repassAPI.Entities;

namespace repassAPI.Data;

public class DatabaseContext: DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Course>()
            .HasOne(c => c.CampusGroup)
            .WithMany(g => g.Courses)
            .HasForeignKey(c => c.CampusGroupId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<CourseStudent>()
            .HasOne(cs => cs.Student)
            .WithMany(u => u.StudingCourses)
            .HasForeignKey(cs => cs.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CourseTeacher>()
            .HasOne(ct => ct.Teacher)
            .WithMany(u => u.TeachingCourses)
            .HasForeignKey(ct => ct.TeacherId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserRoles>()
            .HasOne(ur => ur.User)
            .WithOne()
            .HasForeignKey<UserRoles>(ur => ur.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.Course)
            .WithMany(c => c.Notifications)
            .HasForeignKey(n => n.CourseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    public DbSet<User> Users { get; init; } = null!; //TODO: а поч тут null!
    public DbSet<RefreshToken> RefreshTokens { get; init; } = null!;
    public DbSet<AccessToken> BannedTokens { get; init; } = null!;
    public DbSet<CampusGroup> CampusGroups { get; init; } = null!;
    public DbSet<Course> Courses { get; init; } = null!;
    public DbSet<CourseStudent> CourseStudents { get; init; } = null!;
    public DbSet<CourseTeacher> CourseTeachers { get; init; } = null!;
    public DbSet<UserRoles> UserRoles { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
}
using Microsoft.EntityFrameworkCore;
using repassAPI.Entities;

namespace repassAPI.Data;

public class DatabaseContext: DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        //TODO: каскадное удаления для user, group
    }
    
    public DbSet<User> Users { get; init; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; init; } = null!;
    public DbSet<AccessToken> BannedTokens { get; init; } = null!;
    public DbSet<CampusGroup> CampusGroups { get; init; } = null!;
    public DbSet<Course> Courses { get; init; } = null!;
    public DbSet<CourseStudent> CourseStudents { get; init; } = null!;
    public DbSet<CourseTeacher> CourseTeachers { get; init; } = null!;
    public DbSet<UserRoles> UserRoles { get; set; } = null!;
}
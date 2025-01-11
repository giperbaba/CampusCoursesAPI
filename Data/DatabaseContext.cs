using Microsoft.EntityFrameworkCore;
using repassAPI.Entities;

namespace repassAPI.Data;

public class DatabaseContext: DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    }
    
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    public DbSet<AccessToken> BannedTokens { get; set; } = null!;
    
    public DbSet<CampusGroup> CampusGroups { get; set; } = null!;
}
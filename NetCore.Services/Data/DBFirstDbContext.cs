using Microsoft.EntityFrameworkCore;
using NetCore_Data.DataModels;

namespace NetCore_Services.Data;

public class DBFirstDbContext : DbContext
{
    public DBFirstDbContext(DbContextOptions<DBFirstDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> User { get; set; } = null!;
    public DbSet<UserRole> UserRole { get; set; } = null!;
    public DbSet<UserRoleByUser> UserRoleByUser { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToTable(name: "User");
        modelBuilder.Entity<UserRole>().ToTable(name: "UserRole");
        modelBuilder.Entity<UserRoleByUser>().ToTable(name: "UserRoleByUser");

        // 복합키
        modelBuilder.Entity<UserRoleByUser>().HasKey(e => new
        {
            e.UserId,e.RoleId
        });

    }
}

using Microsoft.EntityFrameworkCore;
using NetCore.DataBase.Data.DBModels;
namespace NetCore_Services.Data;

public class DbFirstDbContext(DbContextOptions<DbFirstDbContext> options) : DbContext(options)
{
    public DbSet<User> User { get; set; } = null!;
    public DbSet<UserRole> UserRole { get; set; } = null!;
    public DbSet<UserRolesByUser> UserRoleByUser { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToTable(name: "User"); 
        modelBuilder.Entity<UserRole>().ToTable(name: "UserRole");
        modelBuilder.Entity<UserRolesByUser>().ToTable(name: "UserRolesByUser");

        // 복합키
        modelBuilder.Entity<UserRolesByUser>().HasKey(e => new
        {
            e.UserId,e.RoleId
        });

    }
}

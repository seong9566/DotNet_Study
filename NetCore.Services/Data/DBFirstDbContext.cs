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
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");
            entity.ToTable("User");
            entity.HasIndex(e => e.UserEmail, "UserEmail").IsUnique();
            entity.Property(e => e.UserId).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(100);
            entity.Property(e => e.UserEmail).HasMaxLength(320);
            entity.Property(e => e.Password).HasMaxLength(130);
            entity.Property(e => e.AccessFailedCount);
            entity.Property(e => e.JoinedUtcDate)
                .HasDefaultValueSql("utc_timestamp()")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");
            entity.ToTable("UserRole");
            entity.Property(e => e.RoleId).HasMaxLength(50);
            entity.Property(e => e.RoleName).HasMaxLength(100);
            entity.Property(e => e.ModifiedUtcDate)
                .HasMaxLength(6)
                .HasDefaultValueSql("utc_timestamp(6)");
        });

        modelBuilder.Entity<UserRolesByUser>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
            entity.ToTable("UserRolesByUser");
            entity.HasIndex(e => new { e.RoleId, e.UserId }, "AK_UserRolesByUser_RoleId_UserId").IsUnique();
            entity.Property(e => e.UserId).HasMaxLength(50);
            entity.Property(e => e.RoleId).HasMaxLength(50);
            entity.Property(e => e.ModifiedUtcDate)
                .HasMaxLength(6)
                .HasDefaultValueSql("utc_timestamp(6)");
            entity.Property(e => e.OwnedUtcDate)
                .HasMaxLength(6)
                .HasDefaultValueSql("utc_timestamp(6)");
            entity.HasOne(d => d.Role).WithMany(p => p.UserRolesByUsers).HasForeignKey(d => d.RoleId);
            entity.HasOne(d => d.User).WithMany(p => p.UserRolesByUsers).HasForeignKey(d => d.UserId);
        });

    }
}

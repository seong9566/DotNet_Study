using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using NetCore.DataBase.Data.DBModels;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace NetCore.DataBase.Data.Repository;

public partial class WorksContext : DbContext
{
    public WorksContext()
    {
    }

    public WorksContext(DbContextOptions<WorksContext> options)
        : base(options)
    {
    }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserRolesByUser> UserRolesByUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=DBFirstDB;user=root;password=stecdev1234!", Microsoft.EntityFrameworkCore.ServerVersion.Parse("9.4.0-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("User");

            entity.HasIndex(e => e.UserEmail, "UserEmail").IsUnique();

            entity.Property(e => e.UserId).HasMaxLength(50);
            entity.Property(e => e.JoinedUtcDate)
                .HasDefaultValueSql("utc_timestamp()")
                .HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(130);
            entity.Property(e => e.UserEmail).HasMaxLength(320);
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("UserRole");

            entity.Property(e => e.RoleId).HasMaxLength(50);
            entity.Property(e => e.ModifiedUtcDate)
                .HasMaxLength(6)
                .HasDefaultValueSql("utc_timestamp(6)");
            entity.Property(e => e.RoleName).HasMaxLength(100);
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

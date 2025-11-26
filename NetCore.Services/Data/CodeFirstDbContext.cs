using Microsoft.EntityFrameworkCore;
using NetCore_Data.DataModels;

namespace NetCore_Services.Data;

// 2. Fluent API 를 작성

// CodeFirstDbContext : 자식 클래스
// DbContext : 부모 클래스
public class CodeFirstDbContext :DbContext
{
    // 생성자 상속 : base(options)
    public CodeFirstDbContext(DbContextOptions<CodeFirstDbContext> options):base(options)
    {
        
    }
    
    // DB 테이블 리스트 지정
    public DbSet<User> Users { get; set; }

    // 메서드 상속, 부모 클래스의 함수를 상속
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // 4가지 작업
        // DB 테이블 이름 변경
        modelBuilder.Entity<User>().ToTable("Users");
        
        // 복합키 지정 
        modelBuilder.Entity<UserRoleByUser>().HasKey(c=> new{c.UserId,c.RoleId});
        
        // 컬럼 기본값 지정
        modelBuilder.Entity<User>(e =>
        {
            e.Property(c => c.IsMembershipWithDrawn).HasDefaultValue(value: false);
        });
        
        // 인덱스 지정
        modelBuilder.Entity<User>().HasIndex(c => new { c.UserEmail });
    }   
}
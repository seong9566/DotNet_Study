using NetCore_Services.Data;
using NetCore_Services.Interfaces;
using NetCore_Services.Svcs;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// .NET 10 기준 의존성 주입 방식
// IUser 인터페이스에 UserService 클래스 인스턴스 주입
// IUser에는 함수명 , 파라미터만 작성
// UserService에서는 IUser 에서 선언한 함수의 구현부가 있음.
builder.Services.AddScoped<IUser,UserService>();

// configuration 선언이 필요
var configuration = builder.Configuration;
// DBMS를 연결 하기 위한 서비스 등록
// Database 접속 정보 , Migrations 프로젝트 지정 
builder.Services.AddDbContext<DBFirstDbContext>(options =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using NetCore.DataBase.Data.Repository;
using NetCore_Services.Interfaces;
using NetCore_Services.Svcs;
using NetCore.Utilities.Utils;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// .NET 10 기준 의존성 주입 방식
// IUser 인터페이스에 UserService 클래스 인스턴스 주입
// IUser에는 함수명 , 파라미터만 작성
// UserService에서는 IUser 에서 선언한 함수의 구현부가 있음.
builder.Services.AddScoped<IUser,UserService>();


var configuration = builder.Configuration;

// DBMS를 연결 하기 위한 서비스 등록
builder.Services.AddDbContext<WorksContext>(options =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

Common.SetDataProtection(
    builder,
    "/Users/stecdev/dp-keys",   // Mac 기준 경로로 변경
    "NetCore",
    Enums.CryptoType.Unmanaged  // Cng CBC는 Mac에서 동작 안 함
);

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

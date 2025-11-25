using NetCore_Services.Interfaces;
using NetCore_Services.Svcs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// .NET 10 기준 의존성 주입 방식
// IUser 인터페이스에 UserService 클래스 인스턴스 주입
// IUser에는 함수명 , 파라미터만 작성
// UserService에서는 IUser 에서 선언한 함수의 구현부가 있음.
builder.Services.AddScoped<IUser,UserService>();

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
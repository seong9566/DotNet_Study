using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore_Data.ViewModels;
using NetCore_Services.Interfaces;

namespace study.Controllers;


/// <summary>
/// MVC 패턴이란?
///
/// Controller는 ViewModel을 입력 받아 처리 후
/// View에 ViewModel을 전달
///
/// ViewModel : 이동 수단의 역할
/// View : Controller에서 받은 데이터들을 보여주는 역할 
/// </summary>

[Authorize(Roles = "AssociateUser,GeneralUser,SuperUser,SystemUser")]
public class MembershipController : Controller
{


    private IUser _user;

    private HttpContext _httpContext = null!;
    
    public MembershipController(IHttpContextAccessor httpContextAccessor,IUser user)
    {
        _httpContext = httpContextAccessor.HttpContext!;
        _user = user;
    }

    #region private methods

    /// <summary>
    /// Local Url인지 외부 Url인지 체크 하는 메서드 
    /// </summary>
    /// <param name="returnUrl"></param>
    /// <returns></returns>
    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction(nameof(MembershipController.Index), "Membership");

        }
    }

    #endregion
    
    
   // GET
   [HttpGet]
   public IActionResult Index()
   {
       return View();
   }
    
   // GET
   [HttpGet]
   [AllowAnonymous] // Controller에 접근 권한을 부여 했지만, 모든 사용자가 접근 할 수 있도록 해주는 어노테이션
   public IActionResult Login(string? returnUrl = null)
   {
       // returnUrl이란?
       ViewData["ReturnUrl"] = returnUrl;
       return View();
   }

    
    
    //POST
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken] 
    public async Task<IActionResult> Login(LoginInfo loginInfo,string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        string message = string.Empty;
        
        if (ModelState.IsValid)
        {
            // 인터페이스로 가져온 함수를 사용 
            if (_user.MatchTheUserInfo(loginInfo))
            {
                var userInfo = _user.GetUserInfo(loginInfo.UserName);
                var roles = _user.GetRolesOwnedByUser(loginInfo.UserName);
                var userTopRole = roles.FirstOrDefault();
                string userDataInfo = userTopRole.Role.RoleName + "|" +
                                      userTopRole.Role.RolePriority.ToString() + "|" +
                                      userInfo.UserName + "|" +
                                      userInfo.UserEmail;
                    
                var identity = new ClaimsIdentity(claims: new[]
                {
                    new Claim(type:ClaimTypes.Name,value:userInfo.UserName),
                    new Claim(type:ClaimTypes.NameIdentifier,value:userInfo.UserId),
                    new Claim(type:ClaimTypes.UserData,value:userDataInfo)
                },authenticationType:CookieAuthenticationDefaults.AuthenticationScheme);
             
                await _httpContext.SignInAsync(scheme:CookieAuthenticationDefaults.AuthenticationScheme,principal:new ClaimsPrincipal(identity:identity),properties:new AuthenticationProperties()
                {
                    // loginInfo 객체의 RememberMe값으로 기억할 것인지 말지 설정
                    IsPersistent = loginInfo.RememberMe,
                    // Remember가 체크 되어 있으면 7일 지속, 아니라면 30분 지속 
                    ExpiresUtc = loginInfo.RememberMe ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddMinutes(30)
                });
                
                // 아이디, 비밀번호 로직 통과 시 메세지를 갖고 Index라는 페이지로 이동
                TempData["Message"] = "로그인 성공";


                return RedirectToLocal(returnUrl);
            }
            else
            {
                // 실패
                message = "로그인 되지 않았습니다.";
            }
        }
        else
        {
            message = "로그인 정보를 올바르게 입력하세요.";
        }
        
        // 모델 State를 추가 
        ModelState.AddModelError(string.Empty,message);
        return View(loginInfo);
    }
    
    // 로그아웃
    [HttpGet("/LogOut")]
    public async Task<IActionResult> Logout()
    {
        await _httpContext.SignOutAsync(scheme:CookieAuthenticationDefaults.AuthenticationScheme);
        TempData["Message"] = "로그아웃이 성공적으로 이루어졌습니다. <br />웹 사이트를 원활하게 이용 하시려면 로그인 하세요.";
        // 성공 시 Index 페이지로 이동
        return RedirectToAction("Index","Membership");
    }

    [HttpGet("Membership/Forbidden")]
    [AllowAnonymous]
    public IActionResult Forbidden()
    {
        bool exists = _httpContext.Request.Query.TryGetValue("returnUrl", out var paramReturnUrl);
        var attempted = exists ? $"{_httpContext.Request.Host.Value}{paramReturnUrl[0]}" : string.Empty;

        ViewData["Message"] = $"귀하는 {attempted} 경로로 접근 시도 했습니다.<br />" +
                              "인증된 사용자도 접근하지 못하는 페이지가 있습니다. <br />" +
                              "담당자에게 해당 페이지의 접근 권한에 대해 문의하세요.";
        
        return View();
    }
}

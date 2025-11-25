using Microsoft.AspNetCore.Mvc;
using NetCore_Data.ViewModels;
using NetCore_Services.Interfaces;
using NetCore_Services.Svcs;
using study.Models;

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
public class MembershipController : Controller
{
    
    /// <summary>
    /// 생성자 주입 방식
    /// </summary>
    private IUser _user;
    public MembershipController(IUser user)
    {
        _user = user;
    }
    
    
   // GET
   [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    
    // GET
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    
    
    //POST
    [HttpPost]
    // 토큰의 POST Data의 데이터를 유효한지 검사
    // 유효 하지 않다면 접근 불가능
    [ValidateAntiForgeryToken] 
    public IActionResult LogIn(LoginInfo loginInfo)
    {
        
        string message = string.Empty;
        var userName = "test";
        var userPassword = "1234";
        
        
        if (ModelState.IsValid)
        {
            // 인터페이스로 가져온 함수를 사용 
            if (_user.MatchTheUserInfo(loginInfo))
            {
                // 아이디, 비밀번호 로직 통과 시 메세지를 갖고 Index라는 페이지로 이동
                TempData["Message"] = "로그인 성공";
                return RedirectToAction("Index","Membership");
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
}
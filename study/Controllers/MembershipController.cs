using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using study.Models;

namespace study.Controllers;


/// <summary>
///  MVC 패턴이란?
///
/// Controller는 ViewModel을 입력 받아 처리 후
/// View에 ViewModel을 전달
///
/// ViewModel : 이동 수단의 역할
/// View : Controller에서 받은 데이터들을 보여주는 역할 
/// </summary>
public class MembershipController : Controller
{
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
        
        // 메세지를 담을 변수 
        string message = string.Empty;
        
        // test를 위한 input 값 
        var userName = "test";
        var userPassword = "1234";
        
        
        if (ModelState.IsValid)
        {
            if (loginInfo.UserName.Equals(userName) && loginInfo.UserPassword.Equals(userPassword))
            {
                // 성공
                
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
using System.ComponentModel.DataAnnotations;

namespace NetCore_Data.ViewModels;

public class LoginInfo
{
    [Required(ErrorMessage = "사용자 아이디를 입력하세요.")]
    [MinLength(6,ErrorMessage = "사용자 아이디는 최소 6자 이상 입력하세요.")]
    [Display(Name = "사용자 아이디")]
    public string UserName { get; set; }
    
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "사용자 아이디를 입력하세요.")]
    [MinLength(2,ErrorMessage = "비밀번호는 최소 2자 이상 입력하세요.")]
    [Display(Name = "사용자 비밀번호")]
    public string UserPassword { get; set; }
    
    [Display(Name = "내 정보 기억")]
    public bool RememberMe{get;set;}
    
}
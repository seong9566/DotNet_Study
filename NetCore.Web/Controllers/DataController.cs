using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using NetCore_Data.ViewModels;

namespace study.Controllers
{
    public class DataController : Controller
    {
        private IDataProtector _protector;

        public DataController(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("NetCore.Data.v1");
        }


        // GET: DataController
        [HttpGet]
        [Authorize(Roles = "GeneralUser,SuperUser,SystemUser")]
        public ActionResult AES()
        {
            return View();
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AES(AesInfo aes)
        {
            string message = string.Empty;

            // Model의 정보가 유효
            if (ModelState.IsValid)
            {
                string userInfo = aes.UserId + aes.UserPassword;
                aes.EncUserInfo = _protector.Protect(userInfo); // 암호화 정보
                aes.DecUserInfo = _protector.Unprotect(aes.EncUserInfo); // 복호화 정보

                ViewData["Message"] = "암복호화 성공";
                return View(aes);
            }
            else
            {
                message = "암복호화를 위한 정보를 올바르게 입력하세요.";
            }
            ModelState.AddModelError(string.Empty, message);
            return View(aes);
            }

        }
    }

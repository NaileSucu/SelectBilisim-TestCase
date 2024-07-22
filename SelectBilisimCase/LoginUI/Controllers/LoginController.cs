using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using LoginUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace LoginUI.Controllers
{
    [AllowAnonymous]

    public class LoginController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        LoginManager loginManager = new LoginManager(new EFUserLoginDal());

        public LoginController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            var res = await _signInManager.PasswordSignInAsync(loginViewModel.Mail, loginViewModel.Password, false, true);

            if (res.Succeeded)
            {
                var loginUser = await _userManager.FindByNameAsync(loginViewModel.Mail);

                if (loginUser != null)
                {
                    if (!loginUser.EmailConfirmed)
                    {
                        HttpContext.Session.SetInt32("UserID", loginUser.UserId);
                        return RedirectToAction("Index", "ConfirmMail");
                    }
                    else
                    {
                        #region Oturumu kapata tıklamadan sonlandırması varsayılarak,aşağıdaki kod bloğu eklenmiştir
                        if (loginManager.TGetList().Any(x => x.UserId == loginUser.UserId))
                            {
                            var lastLog = loginManager.TGetByID(loginManager.TGetList().Where(x => x.UserId == loginUser.UserId).Last().LoginId);
                            if (lastLog.LogoutTime == null)
                            {
                                lastLog.LogoutTime = DateTime.Now;
                                loginManager.TUpdate(lastLog);
                            }
                        }
                        #endregion
                        UserLogins userLogins = new UserLogins()
                        {
                            UserId = loginUser.UserId,
                            LoginTime = DateTime.Now,
                            LogoutTime = null
                        };
                        loginManager.TAdd(userLogins);

                        if (User.Identity != null && User.Identity.IsAuthenticated)
                        {
                            if (User.IsInRole("Admin"))
                            {
                                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                            }
                            else if (User.IsInRole("User"))
                            {
                                return RedirectToAction("Index", "MainPage");
                            }
                        }
                    }
                }

            }
            return View();

        }
        public async Task<IActionResult> LogOut()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var loginUser = await _userManager.FindByNameAsync(User.Identity.Name);
                UserLogins lastLog=new UserLogins();
                if (loginManager.TGetList().Any(x => x.UserId == loginUser.UserId))
                {
                    lastLog = loginManager.TGetByID(loginManager.TGetList().Where(x => x.UserId == loginUser.UserId).Last().LoginId);                  
                }
                lastLog.LogoutTime = DateTime.Now;
                loginManager.TUpdate(lastLog);
            }


            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }
    }
}
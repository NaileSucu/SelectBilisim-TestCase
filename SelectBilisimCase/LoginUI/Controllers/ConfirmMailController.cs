using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using LoginUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LoginUI.Controllers
{
    public class ConfirmMailController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public ConfirmMailController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            int userId = HttpContext.Session.GetInt32("UserID") ?? 0;
            ViewBag.UserId = userId;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(ConfirmMailViewModel confirmMail)
        {
            confirmMail.UserId = confirmMail.UserId != 0 ? confirmMail.UserId:Convert.ToInt32(HttpContext.Session.GetInt32("UserID"));
            var user = await _userManager.FindByIdAsync(confirmMail.UserId.ToString());
            if (user != null)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                if (user.ConfirmCode == confirmMail.ConfirmCode)
                {
                    await _signInManager.RefreshSignInAsync(user);
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
            return View();

        }

    }
}

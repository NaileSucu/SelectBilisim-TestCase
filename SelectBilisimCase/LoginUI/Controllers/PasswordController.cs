using EntityLayer.Concrete;
using LoginUI.Models;
using LoginUI.Provider;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MimeKit;

namespace LoginUI.Controllers
{
    public class PasswordController : Controller
    {
        private readonly UserManager<User> _userManager;

        public PasswordController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel passwordVM)
        {
            var user=await _userManager.FindByNameAsync(passwordVM.Mail);
            string passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResetTokenLink = Url.Action("ResetPassword", "Password",
            new
            {
              userID = user.Id,
              token = passwordResetToken,
            }, HttpContext.Request.Scheme);
            var builder = new BodyBuilder();
            builder.TextBody = passwordResetTokenLink;

            SendMail.Send("xlnxoyehtlrgtgtw", user.FirstName + " " + user.LastName, user.Email, "Şifre Değişiklik Talebi", builder);


            return RedirectToAction("Index", "Login");
        }
        [HttpGet]
        public IActionResult ResetPassword(string userid,string token)
        {
            HttpContext.Session.SetString("UserID", userid);
            HttpContext.Session.SetString("Token", token);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordVM)
        {
            var userid = HttpContext.Session.GetString("UserID");
            var token = HttpContext.Session.GetString("Token");

            if(!string.IsNullOrEmpty(userid) || !string.IsNullOrEmpty(token))
            {
               var user=await _userManager.FindByIdAsync(userid);
               var res= await _userManager.ResetPasswordAsync(user, token, resetPasswordVM.Password);
                if(res.Succeeded)
                    return RedirectToAction("Index", "Login");

            }
            return View();
        }
    }
}

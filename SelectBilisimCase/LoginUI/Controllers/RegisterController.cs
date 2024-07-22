using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using LoginUI.Models;
using LoginUI.Provider;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace LoginUI.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> roleManager;

        public RegisterController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(RegisterViewModel register)
        {
            if (ModelState.IsValid && register.Password == register.ConfirmPassword)
            {
                Random random = new Random();
                User user = new User()
                {
                    UserName = register.Email,
                    EmailConfirmed = false,
                    RegisterDate = DateTime.Now,
                    FirstName = register.Name,
                    LastName = register.SurName,
                    Email = register.Email,
                    ConfirmCode = random.Next(100000, 1000000).ToString()
                };
                var res=await _userManager.CreateAsync(user,register.Password);     
                if (res.Succeeded)
                {
                    var loginUser = await _userManager.FindByNameAsync(user.Email);
                    try
                    {
                        if (!await roleManager.RoleExistsAsync("User"))
                            await roleManager.CreateAsync(new Role("User"));
                        await _userManager.AddToRoleAsync(loginUser, "User");

                    }
                    catch (Exception ex)
                    {

                    }
                    user.UserId = loginUser.Id;
                    await _userManager.UpdateAsync(user);
                    var builder = new BodyBuilder();
                    builder.TextBody = "Kayıt işlemini tamamlamak için onay kodunuz: " + user.ConfirmCode;
                    SendMail.Send("omkohvojxbovnqpt", register.Name + " " + register.SurName,register.Email,"Onay Kodu",builder);

                    HttpContext.Session.SetInt32("UserID", user.UserId);
                    return RedirectToAction("Index","ConfirmMail");
                }
                else
                {
                    
                        foreach (var item in res.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    
                }

            }
            return View(register);
        }
    }
}

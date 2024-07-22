using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LoginUI.Areas.Admin.ViewComponents
{
    public class Navbar : ViewComponent
    {
        private readonly UserManager<User> _userManager;

        public Navbar(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.Values = values.FirstName.Substring(0,1) + values.LastName.Substring(0, 1);
            return View();
        }
    }
}

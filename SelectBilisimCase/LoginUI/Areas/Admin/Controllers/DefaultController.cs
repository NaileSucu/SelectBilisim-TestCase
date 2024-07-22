using Microsoft.AspNetCore.Mvc;

namespace LoginUI.Areas.Admin.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoginUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class DashboardController : Controller
    {
        Context context = new Context();

        [Area("Admin")]
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.SelectedDay = "Rapor Günü Seçin";

            ViewBag.NotVerifiedUserCountToday = context.Users.Where(x => x.EmailConfirmed == false).ToList().Where(x => (DateTime.Now - x.RegisterDate).TotalHours <= 24).Count();

             ViewBag.OnlineUserCountToday = context.UserLogin.Where(x => x.LogoutTime==null).ToList().Where(x=>x.LoginTime<= DateTime.Now).ToList().Count;

            ViewBag.RegisteredUserCountToday = context.Users.ToList().Where(x => (DateTime.Now - x.RegisterDate).TotalHours <= 24).Count();
          
             var loggedInTodayUsers = context.UserLogin.ToList().Where(x => x.LogoutTime != null &&  ((DateTime.Now - x.LoginTime).TotalHours <= 24));

             var userCount = loggedInTodayUsers.Select(x => x.UserId).Distinct().Count();
             double sessionDuration = 0;
            foreach (var user in loggedInTodayUsers)
            {
                 sessionDuration = (user.LogoutTime - user.LoginTime)?.TotalSeconds ?? 0;
            }

            ViewBag.AvgSessionTimeToday = sessionDuration > 0 ? (sessionDuration / userCount).ToString("N0") : "0";
            return View();
        }
        [HttpPost]
        public IActionResult Index(string selectedDay)
        {
            int selectedDayNum = Convert.ToInt32(selectedDay);
            ViewBag.SelectedDay = selectedDay;
            ViewBag.NotVerifiedUserCount = context.Users.Where(x => x.EmailConfirmed == false).ToList().Where(x => (DateTime.Now.AddDays(-selectedDayNum) - x.RegisterDate).TotalDays <= selectedDayNum).Count();

            ViewBag.OnlineUserCount = context.UserLogin.Where(x => x.LogoutTime == null).ToList().Where(x => x.LoginTime <= DateTime.Today.AddDays(-selectedDayNum)).ToList().Count;

            ViewBag.RegisteredUserCount = context.Users.ToList().Where(x => (DateTime.Now - x.RegisterDate).TotalDays <= selectedDayNum).Count();

            var loggedInTodayUsers = context.UserLogin.ToList().Where(x => x.LogoutTime != null && ((DateTime.Now - x.LoginTime).TotalDays <= selectedDayNum));

            var userCount = loggedInTodayUsers.Select(x => x.UserId).Distinct().Count();
            double sessionDuration = 0;
            foreach (var user in loggedInTodayUsers)
            {
                sessionDuration = (user.LogoutTime - user.LoginTime)?.TotalSeconds ?? 0;
            }
            ViewBag.AvgSessionTime = sessionDuration>0 ? (sessionDuration / userCount).ToString("N0"): "0";

            return View(); 
        }
    }
}

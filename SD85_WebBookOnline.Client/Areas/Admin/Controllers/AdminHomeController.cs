using Microsoft.AspNetCore.Mvc;

namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class AdminHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult BookMannager()
        {
            return RedirectToAction("AllBookManager", "BookManager", new { area = "Admin" });
        }
        public IActionResult VoucherManager()
        {
            return RedirectToAction("AllVoucherManager", "VoucherManager",new {area = "Admin"});
        }
    }
}

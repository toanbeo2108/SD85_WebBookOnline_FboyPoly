using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace SD85_WebBookOnline.Client.Areas.Customer.Controllers
{
    public class CustomerHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

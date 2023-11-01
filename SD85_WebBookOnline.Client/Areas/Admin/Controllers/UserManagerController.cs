using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;

namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class UserManagerController : Controller
    {
        private HttpClient _httpClient;
        public UserManagerController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ViewBag.JwtToken = token;
            return View();
        }
    }
}

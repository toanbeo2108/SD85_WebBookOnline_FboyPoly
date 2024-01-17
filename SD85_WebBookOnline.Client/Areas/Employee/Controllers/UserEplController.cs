using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http.Headers;

namespace SD85_WebBookOnline.Client.Areas.Employee.Controllers
{
    public class UserEplController : Controller
    {
        private HttpClient _httpClient;
        public UserEplController()
        {
            _httpClient = new HttpClient();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUserEpl()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"https://localhost:7079/api/user/GetUsersByRole?roleName=User";
            var response = await _httpClient.GetAsync(url);
            string apiDataUser = await response.Content.ReadAsStringAsync();
            var ListUser = JsonConvert.DeserializeObject<List<User>>(apiDataUser);
            return View(ListUser);
        }
    }
}

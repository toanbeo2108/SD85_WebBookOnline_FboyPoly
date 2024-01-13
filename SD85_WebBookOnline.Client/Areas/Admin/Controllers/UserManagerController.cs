using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Text;

namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class UserManagerController : Controller
    {
        private HttpClient _httpClient;
        public UserManagerController()
        {
            _httpClient = new HttpClient();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"https://localhost:7079/api/user/GetUsersByRole?roleName=User";
            var response = await _httpClient.GetAsync(url);
            string apiDataUser = await response.Content.ReadAsStringAsync();
            var ListUser = JsonConvert.DeserializeObject<List<User>>(apiDataUser);
            return View(ListUser);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEmployee()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"https://localhost:7079/api/user/GetUsersByRole?roleName=Employee";
            var response = await _httpClient.GetAsync(url);
            string apiDataUser = await response.Content.ReadAsStringAsync();
            var ListUser = JsonConvert.DeserializeObject<List<User>>(apiDataUser);
            return View(ListUser);
        }
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"https://localhost:7079/api/user/GetUsersById?id=" + id;
            var httpClient = new HttpClient();
            var response = await _httpClient.GetAsync(url);
            string apiDataUser = await response.Content.ReadAsStringAsync();
            var User = JsonConvert.DeserializeObject<User>(apiDataUser);
            return View(User);
        }
        // Mở form
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"https://localhost:7079/api/user/GetUsersById?id=" + id;
            var httpClient = new HttpClient();
            var response = await _httpClient.GetAsync(url);
            string apiDataUser = await response.Content.ReadAsStringAsync();
            var User = JsonConvert.DeserializeObject<User>(apiDataUser);
            return View(User);
        }

        [HttpPost]
        public async Task<IActionResult> Update(User user, IFormFile imageFile)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string apiURL = $"https://localhost:7079/api/user/UpdateUser";

            if (imageFile != null && imageFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imageFile.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                user.Avatar = imageFile.FileName;
            }
            else
            {
                var url = $"https://localhost:7079/api/user/GetUsersById?id=" + user.Id;
                var responseUrl = await _httpClient.GetAsync(url);
                string apiDataUser = await responseUrl.Content.ReadAsStringAsync();
                var existingUser = JsonConvert.DeserializeObject<User>(apiDataUser);
                user.Avatar = existingUser.Avatar;
            }
            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(apiURL, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllUser");
            }
            else
            {
                return BadRequest();
            }
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return View();
        }

    }
}

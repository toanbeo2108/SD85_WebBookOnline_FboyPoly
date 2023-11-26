using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using SD85_WebBookOnline.Share.Models;
using System.Text;

namespace SD85_WebBookOnline.Client.Areas.Customer.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;
        public UserController()
        {
            _httpClient = new HttpClient();
        }
        [HttpGet]
        public async Task<IActionResult> Account()
        {
            var UserId = Request.Cookies["UserID"];
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"https://localhost:7079/api/user/GetUsersById?id=" + UserId;
            var response = await _httpClient.GetAsync(url);
            string apiDataUser = await response.Content.ReadAsStringAsync();
            var User = JsonConvert.DeserializeObject<User>(apiDataUser);
            return View(User);
        }
        // Mở form
        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var UserId = Request.Cookies["UserID"];

            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"https://localhost:7079/api/user/GetUsersById?id=" + UserId;
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
                return RedirectToAction("Account");
            }
            else
            {
                return BadRequest();
            }
        }
    }
}

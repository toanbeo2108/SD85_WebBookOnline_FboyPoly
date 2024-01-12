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
        public async Task<IActionResult> Update(User u,IFormFile imageFile)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var UserId = Request.Cookies["UserID"];
            var urlUser = $"https://localhost:7079/api/user/GetUsersById?id=" + UserId;
            var responseUser = await _httpClient.GetAsync(urlUser);
            string apiDataUser = await responseUser.Content.ReadAsStringAsync();
            var User = JsonConvert.DeserializeObject<User>(apiDataUser);
            User.UserName = u.UserName;
            User.PhoneNumber = u.PhoneNumber;
            User.Email = u.Email;
            User.Country = u.Country;

            if (imageFile != null && imageFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imageFile.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                User.Avatar = imageFile.FileName;
            }
            else
            {
                User.Avatar = User.Avatar;
            }

            string apiURL = $"https://localhost:7079/api/user/UpdateUser";
            var content = new StringContent(JsonConvert.SerializeObject(User), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(apiURL, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Account");
            }
            else
            {
                return BadRequest("Lỗi cập nhật User");
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http.Headers;
using System.Text;

namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class RatingManagerController : Controller
    {
        
        private HttpClient _httpClient;
        public RatingManagerController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]

        public async Task<IActionResult> Book_AllRating(Guid id)
          {

            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            var url = $"https://localhost:7079/api/user/GetAllUser";
            var response = await _httpClient.GetAsync(url);
            string apiDataUser = await response.Content.ReadAsStringAsync();
            var ListUser = JsonConvert.DeserializeObject<List<User>>(apiDataUser);
            ViewBag.listUser = ListUser;


            var urlRating = $"https://localhost:7079/api/Rating/GetAllRating";
            var httpClient = new HttpClient();
            var responRating = await _httpClient.GetAsync(urlRating);
            string apiDataRating = await responRating.Content.ReadAsStringAsync();
            var lstRating = JsonConvert.DeserializeObject<List<Rating>>(apiDataRating);
            var Rating = lstRating.FirstOrDefault(x => x.IdBook == id);
            if (Rating == null)
            {
                return BadRequest("Sản phẩm này ko có bình luận nào");
            }
            else
            {
                ViewBag.lstRating = lstRating;
                return View(lstRating);
            }

        }


        

        [HttpGet]
        public async Task<IActionResult> RatingUpdate(Guid idrat)
        {

            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = $"https://localhost:7079/api/user/GetAllUser";
            var response = await _httpClient.GetAsync(url);
            string apiDataUser = await response.Content.ReadAsStringAsync();
            var ListUser = JsonConvert.DeserializeObject<List<User>>(apiDataUser);
            ViewBag.ListUser = ListUser;

            var urlRating = $"https://localhost:7079/api/Rating/GetAllRating";
            var responRating = await _httpClient.GetAsync(urlRating);
            string apiDataRating = await responRating.Content.ReadAsStringAsync();
            var lstRating = JsonConvert.DeserializeObject<List<Rating>>(apiDataRating);
            var Rating = lstRating.FirstOrDefault(x => x.ID == idrat);
            ViewBag.Rating = Rating;

            return View(Rating);
        }

        [HttpPost]
        public async Task<IActionResult> RatingUpdate(Guid idrat, Rating rt)
        {

            var urlRating = $"https://localhost:7079/api/Rating/UpdateRating/{idrat}";
            var content = new StringContent(JsonConvert.SerializeObject(rt), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlRating, content);
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var urlRatingget = $"https://localhost:7079/api/Rating/GetAllRating";
            var responRating = await _httpClient.GetAsync(urlRatingget);
            string apiDataRating = await responRating.Content.ReadAsStringAsync();
            var lstRating = JsonConvert.DeserializeObject<List<Rating>>(apiDataRating);
            var Rating = lstRating.FirstOrDefault(x => x.ID == idrat);
            
            return RedirectToAction("Book_AllRating", "RatingManager",new {Area = "Admin" }); 
            
        }
        [HttpGet]
        public async Task<IActionResult> RatingDelete(Guid idrat)
        {

            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlVoucher = $"https://localhost:7079/api/Rating/DeleteRating/{idrat}";
            var respon = await _httpClient.DeleteAsync(urlVoucher);
            return RedirectToAction("Book_AllRating", "RatingManager", new { Area = "Admin" });

        }



    }
}

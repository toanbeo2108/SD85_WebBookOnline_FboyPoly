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

        public async Task<IActionResult> Book_AllRating(Guid idbookrt)
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
            var lstRatingok = lstRating.Where(x => x.IdBook == idbookrt).ToList();
            if (lstRatingok.Count == 0)
            {
                return RedirectToAction("MessageNull", "RatingManager", new { area = "Admin" });
            }
            else
            {
                ViewBag.lstRatingok = lstRatingok;
                return View(lstRatingok);
            }

        }

        [HttpGet, Route("delete_rating/{id}")]
        public async Task<IActionResult> RatingDelete(Guid id)
        {
            string _mess = "";
            bool _stt = false;

            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlVoucher = $"https://localhost:7079/api/Rating/DeleteRating/{id}";
            var respon = await _httpClient.DeleteAsync(urlVoucher);
            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
            {

                _stt = true;
                _mess = "Xóa thanh cong!";

            }
            else
            {
                _stt = false;
                _mess = "Xóa that bai!";
            }
            return Json(new
            {
                status = _stt,
                message = _mess
            });

        }



    }
}

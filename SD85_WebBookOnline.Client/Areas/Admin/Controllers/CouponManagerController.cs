using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http.Headers;
using System.Text;

namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class CouponManagerController : Controller
    {
        private HttpClient _httpClient;
        bool _stt = false;
        string _mess = "";
        object _data = null;
        public CouponManagerController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        //https://localhost:7079/api/Coupon/CreateCoupon
        //https://localhost:7079/api/Coupon/UpdateCoupon/{id}
        //https://localhost:7079/api/Coupon/CreateCoupon/{id}
        public async Task<IActionResult> AllCouponManager()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/Coupon/GetAllCoupon";
            var httpClient = new HttpClient();
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Coupon>>(apiDataBook);
            return View(lstBook);
        }
        [HttpGet]
        public IActionResult CreateCoupon()
        {
            return View();
        }
        [HttpPost,Route("add-Coupon")]
        public async Task<IActionResult> CreateCoupon(Coupon bk)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            bk.CouponID = Guid.NewGuid();
            //bk.CreateDate = DateTime.Now;
            var urlBook = $"https://localhost:7079/api/Coupon/CreateCoupon?couponame={bk.CouponName}&percentdiscount={bk.PercentDiscount}&startDate={bk.StartDate}&enddate={bk.EndDate}&description={bk.Description}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(urlBook, content);
            //if (respon.IsSuccessStatusCode)
            //{
            //    return RedirectToAction("AllCouponManager", "CouponManager", new { area = "Admin" });
            //}
            //TempData["erro message"] = "thêm thất bại";
            //return View();
            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _stt = true;
                _mess = "Thêm thành công !";
            }
            else
            {
                _stt = false;
                _mess = "Thêm thất bại";
            }
            return Json(new
            {
                status = _stt,
                message = _mess,
            });
        }
        [HttpGet,Route("Detail-coupon/{id}")]
        public async Task<IActionResult> CouponDetail(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/Coupon/GetAllCoupon";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Coupon>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.CouponID == id);
            //if (Book == null)
            //{
            //    return BadRequest();
            //}
            //else
            //{
            //    return View(Book);
            //}
            if (responBook.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _stt = true;
                _mess = "";
                _data = Book;
            }
            else
            {
                _stt = false;
                _mess = "Không tìm thấy thông tin";
            }
            return Json(new
            {
                status = _stt,
                message = _mess,
                data = _data
            });
        }
        [HttpGet]
        public async Task<IActionResult> UpdateCoupon(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/Coupon/GetAllCoupon";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Coupon>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.CouponID == id);
            if (Book == null)
            {
                return BadRequest();
            }
            else
            {
                return View(Book);
            }
        }
        [HttpPost,Route("Update-coupon/{id}")]
        public async Task<IActionResult> UpdateCoupon(Guid id, Coupon vc)
        {
            var urlBook = $"https://localhost:7079/api/Coupon/UpdateCoupon/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            //if (!respon.IsSuccessStatusCode)
            //{
            //    return BadRequest();
            //}
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //return RedirectToAction("AllCouponManager", "CouponManager", new { area = "Admin" });
            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _stt = true;
                _mess = "Cập nhật thành công !";
            }
            else
            {
                _stt = false;
                _mess = "Cập nhật thất bại";
            }
            return Json(new
            {
                status = _stt,
                message = _mess,
            });

        }
    }  
}

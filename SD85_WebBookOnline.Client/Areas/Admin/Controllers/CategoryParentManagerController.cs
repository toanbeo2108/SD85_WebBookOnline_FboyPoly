using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http.Headers;

namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class CategoryParentManagerController : Controller
    {
        private readonly HttpClient _httpClient;
        public CategoryParentManagerController()
        {
            _httpClient = new HttpClient();
        }
        [HttpGet]
        public async Task<IActionResult> DanhSachTheLoaiChiTiet()
        {
            string urlctd = $"https://localhost:7079/api/CategoryParent/GetAllCategoryParents";
            var responctd = await _httpClient.GetAsync(urlctd);
            string apidata = await responctd.Content.ReadAsStringAsync();
            var lstctd = JsonConvert.DeserializeObject<List<CategoryParent>>(apidata);
            ViewBag.lstctd = lstctd;
            return View();
        }
        public IActionResult CreateCategoryDetail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategoryDetail(CategoryParent ctd)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ctd.CategoryParentID = Guid.NewGuid();
            var urlCTD = $"https://localhost:7079/api/CategoryParent/CreateCategoryDetail?";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> thongTinTheLoaiSachChiTiet(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string urlctd = $"https://localhost:7079/api/CategoryParent/GetAllCategoryParents";
            var responctd = await _httpClient.GetAsync(urlctd);
            string apidata = await responctd.Content.ReadAsStringAsync();
            var lstctd = JsonConvert.DeserializeObject<List<CategoryParent>>(apidata);
            if(lstctd == null)
            {
                return NotFound("Khong ton tai the loai chi tiet sach");
            }
            var ctd = lstctd.FirstOrDefault(x => x.CategoryParentID == id);
            if(ctd == null)
            {
                return NotFound("Khong ton tai the loai chi tiet sach");
            }
            return View(ctd);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateCategoryDetail(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string urlctd = $"https://localhost:7079/api/CategoryParent/GetAllCategoryParents";
            var responctd = await _httpClient.GetAsync(urlctd);
            string apidata = await responctd.Content.ReadAsStringAsync();
            var lstctd = JsonConvert.DeserializeObject<List<CategoryParent>>(apidata);
            if (lstctd == null)
            {
                return NotFound("Khong ton tai the loai chi tiet sach");
            }
            var ctd = lstctd.FirstOrDefault(x => x.CategoryParentID == id);
            if (ctd == null)
            {
                return NotFound("Khong ton tai the loai chi tiet sach");
            }
            return View();
        }
    }
}

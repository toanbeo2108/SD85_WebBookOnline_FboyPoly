using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Drawing.Printing;
using System.Net.Http.Headers;
using System.Text;

namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class PostBannerManagerController : Controller
    {
        private HttpClient _httpClient;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PostBannerManagerController(IWebHostEnvironment hostEnvironment)
        {
            _httpClient = new HttpClient();
            _hostingEnvironment = hostEnvironment;      
        }
        public int pageSize = 6;
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AllPostBannerManager()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/Postbanner/GetAll-PostBanner";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<PostBanner>>(apiDataBook);
            return View(lstBook);

        }
        [HttpGet]
        public IActionResult CreatePostBanner()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreatePostBanner(PostBanner bk, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photoBooks", imageFile.FileName);
                var stream = new FileStream(path, FileMode.Create);
                imageFile.CopyTo(stream);
                bk.Images = imageFile.FileName;
            }
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            bk.PostID = Guid.NewGuid();
            //bk.CreateDate = DateTime.Now;

            var urlBook = $"https://localhost:7079/api/Postbanner/Create-postBaner?Images={bk.Images}&PostDate={bk.PostDate}&Title={bk.Title}&Content={bk.Content}&Status={bk.Status}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(urlBook, content);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("AllCategoryManager", "CategoryManager", new { area = "Admin" });
            }
            TempData["erro message"] = "thêm thất bại";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeletePostBanner(Guid id)
        {
            if (await TryDeletePostBanner(id))
            {
                return RedirectToAction("AllPostBannerManager", "PostBannerManager", new { area = "Admin" });
            }

            TempData["ErrorMessage"] = "Xóa Combo không thành công"; // Thêm thông báo lỗi
            return RedirectToAction("AllPostBannerManager", "PostBannerManager", new { area = "Admin" });
        }

        private async Task<bool> TryDeletePostBanner(Guid id)
        {
            try
            {
                var token = Request.Cookies["Token"];
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var urlCombo = $"https://localhost:7079/api/Postbanner/Delete-postBaner/{id}";

                var respon = await _httpClient.DeleteAsync(urlCombo);

                return respon.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [HttpGet]
        public async Task<IActionResult> PostBannerDetail(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/Postbanner/GetAll-PostBanner";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<PostBanner>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.PostID == id);
            if (Book == null)
            {
                return BadRequest();
            }
            else
            {
                return View(Book);
            }
        }
        [HttpGet]
        public async Task<IActionResult> UpdatePostBanner(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlCombo = $"https://localhost:7079/api/Postbanner/GetAll-PostBanner";
            var responCombo = await _httpClient.GetAsync(urlCombo);
            string apiDataCombo = await responCombo.Content.ReadAsStringAsync();
            var lstCombo = JsonConvert.DeserializeObject<List<PostBanner>>(apiDataCombo);
            var combo = lstCombo.FirstOrDefault(x => x.PostID == id);
            if (combo == null)
            {
                return BadRequest();
            }
            else
            {
                return View(combo);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePostBanner(Guid id, PostBanner cb, IFormFile imageFile)
        {
            var urlCombo = $"https://localhost:7079/api/Postbanner/Update-postBaner/{id}";

            // Kiểm tra xem id đã được cung cấp có khớp với ComboID hay không
            if (id != cb.PostID)
            {
                return BadRequest();
            }

            // Kiểm tra xem tệp hình ảnh mới có được cung cấp hay không
            if (imageFile != null && imageFile.Length > 0)
            {
                // Xác định đường dẫn lưu trữ hình ảnh mới
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imageFile.FileName);

                // Lưu tệp hình ảnh mới
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Cập nhật thuộc tính 'Image' với tên tệp hình ảnh mới
                cb.Images = imageFile.FileName;
            }

            // Chuyển đổi dữ liệu Combo thành chuỗi JSON
            var content = new StringContent(JsonConvert.SerializeObject(cb), Encoding.UTF8, "application/json");

            // Gửi yêu cầu PUT đến API để cập nhật Combo
            var response = await _httpClient.PutAsync(urlCombo, content);

            if (response.IsSuccessStatusCode)
            {
                // Nếu cập nhật thành công, chuyển hướng đến trang quản lý Combo
                return RedirectToAction("AllComboManager", "ComboManager", new { area = "Admin" });
            }
            else
            {
                // Nếu cập nhật thất bại, hiển thị thông báo lỗi và trở về trang chỉnh sửa Combo
                TempData["ErrorMessage"] = "Cập nhật không thành công";
                return View(cb);
            }
        }
    }
}

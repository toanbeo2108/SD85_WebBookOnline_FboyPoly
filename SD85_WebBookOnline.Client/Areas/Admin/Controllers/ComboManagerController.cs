using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting; // Thêm namespace để sử dụng IWebHostEnvironment
using Microsoft.AspNetCore.Http; // Thêm namespace để sử dụng IFormFile
using System.Net.Http.Headers;

namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class ComboManagerController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ComboManagerController(IWebHostEnvironment webHostEnvironment)
        {
            _httpClient = new HttpClient();
            _webHostEnvironment = webHostEnvironment;
        }
        

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AllComboManager()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlCombo = $"https://localhost:7079/api/Combo/GetAllCombo";
            var httpClient = new HttpClient();
            var responCombo = await _httpClient.GetAsync(urlCombo);
            string apiDataCombo = await responCombo.Content.ReadAsStringAsync();
            var lstCombo = JsonConvert.DeserializeObject<List<Combo>>(apiDataCombo);
            return View(lstCombo);
        }
        [HttpGet]
        public async Task<IActionResult> ComboDetail(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlCombo = $"https://localhost:7079/api/Combo/GetAllCombo";
            var responCombo = await _httpClient.GetAsync(urlCombo);
            string apiDataCombo = await responCombo.Content.ReadAsStringAsync();
            var lstCombo = JsonConvert.DeserializeObject<List<Combo>>(apiDataCombo);
            var combo = lstCombo.FirstOrDefault(x => x.ComboID == id);
            if (combo == null)
            {
                return BadRequest();
            }
            else
            {
                return View(combo);
            }
        }

        public IActionResult CreateCombo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCombo(Combo cb, IFormFile imageFile)
        {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imageFile.FileName);
                    var stream = new FileStream(path, FileMode.Create);
                    imageFile.CopyTo(stream);
                    cb.Image = imageFile.FileName;
                }
                var urlCombo = $"https://localhost:7079/api/Combo/CreateCombo?comboname={cb.ComboName}&price={cb.Price}&status={cb.Status}&image={cb.Image}";
                var token = Request.Cookies["Token"];
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var content = new StringContent(JsonConvert.SerializeObject(cb), Encoding.UTF8, "application/json");
                var respon = await _httpClient.PostAsync(urlCombo, content);
                if (respon.IsSuccessStatusCode)
                {
                    return RedirectToAction("AllComboManager", "ComboManager", new { area = "Admin" });
                }
                else
                {
                    TempData["ErrorMessage"] = "Thêm Thất Bại";
                    return View();
                }
            }
           
        [HttpPost]
        public async Task<IActionResult> DeleteCombo(Guid id)
        {
            if (await TryDeleteCombo(id))
            {
                return RedirectToAction("AllComboManager", "ComboManager", new { area = "Admin" });
            }

            TempData["ErrorMessage"] = "Xóa Combo không thành công"; // Thêm thông báo lỗi
            return RedirectToAction("AllComboManager", "ComboManager", new { area = "Admin" });
        }

        private async Task<bool> TryDeleteCombo(Guid id)
        {
            try
            {
                var token = Request.Cookies["Token"];
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var urlCombo = $"https://localhost:7079/api/Combo/DeleteCombo/{id}";

                var respon = await _httpClient.DeleteAsync(urlCombo);

                return respon.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [HttpGet]
        public async Task<IActionResult> UpdateCombo(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlCombo = $"https://localhost:7079/api/Combo/GetAllCombo";
            var responCombo = await _httpClient.GetAsync(urlCombo);
            string apiDataCombo = await responCombo.Content.ReadAsStringAsync();
            var lstCombo = JsonConvert.DeserializeObject<List<Combo>>(apiDataCombo);
            var combo = lstCombo.FirstOrDefault(x => x.ComboID == id);
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
        public async Task<IActionResult> UpdateCombo(Guid id, Combo cb, IFormFile imageFile)
        {
            var urlCombo = $"https://localhost:7079/api/Combo/UpdateCombo/{id}";

            // Kiểm tra xem id đã được cung cấp có khớp với ComboID hay không
            if (id != cb.ComboID)
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
                cb.Image = imageFile.FileName;
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

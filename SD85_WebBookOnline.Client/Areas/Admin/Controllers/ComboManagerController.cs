﻿using Microsoft.AspNetCore.Authorization;
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
using System.Net;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using System.Transactions;

namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class ComboManagerController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClient _httpClientz;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public List<ComboItem> ComboItems { get; set; } = new List<ComboItem>();

        public ComboManagerController(IWebHostEnvironment webHostEnvironment)
        {
            _httpClient = new HttpClient();
            _httpClientz = new HttpClient();
            _webHostEnvironment = webHostEnvironment;
        }

        [AutoValidateAntiforgeryToken]        
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
            string json = Request.Cookies["lstComboItem"];
            if (json != null)
            {
                List<ComboItem> myList = JsonConvert.DeserializeObject<List<ComboItem>>(json);
                ViewBag.ListComboItem = myList;
                return View(lstCombo);
            }
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
                var urlComboItem = $"https://localhost:7079/api/ComboItem/GetAll-ComboItem";
                var responComboItem = await _httpClient.GetAsync(urlComboItem);
                string apiDataComboItem = await responComboItem.Content.ReadAsStringAsync();
                var lstComboItem = JsonConvert.DeserializeObject<List<ComboItem>>(apiDataComboItem);
                var lstComboItemOfCombo = lstComboItem.Where(x => x.ComboID == combo.ComboID).ToList();
                if (lstComboItemOfCombo == null)
                {
                    return NotFound();
                }
                ViewBag.lstComboItemOfCombo = lstComboItemOfCombo;
                return View(combo);

            }
        }

        public async Task<IActionResult> CreateCombo()
        {
            var urlBook = "https://localhost:7079/api/Book/get-all-book";
            var httpClient = new HttpClient();
            var responseBook = await httpClient.GetAsync(urlBook);
            if (!responseBook.IsSuccessStatusCode)
            {
                return BadRequest("Lỗi khi tải danh sách sách.");
            }

            string apiDataBook = await responseBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            ViewBag.lstBook = lstBook;

            string json = Request.Cookies["lstComboItem"];
            if (json != null)
            {
                List<ComboItem> myList = JsonConvert.DeserializeObject<List<ComboItem>>(json);
                ViewBag.ListComboItem = myList;
            }

            var urlCombo = "https://localhost:7079/api/Combo/GetAllCombo"; // Đọc danh sách Combo từ cơ sở dữ liệu
            var responseCombo = await httpClient.GetAsync(urlCombo);
            if (responseCombo.IsSuccessStatusCode)
            {
                string apiDataCombo = await responseCombo.Content.ReadAsStringAsync();
                var lstCombo = JsonConvert.DeserializeObject<List<Combo>>(apiDataCombo);
                ViewBag.ListCombo = lstCombo;
            }

            return View();
        }

        public async Task<IActionResult> AddToCombo(Guid id)
        {
            var urlBook = "https://localhost:7079/api/Book/get-all-book";
            var httpClient = new HttpClient();
            var responseBook = await httpClient.GetAsync(urlBook);
            if (!responseBook.IsSuccessStatusCode)
            {
                return BadRequest("Lỗi khi tải danh sách sách.");
            }

            string apiDataBook = await responseBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            ViewBag.lstBook = lstBook;

            var book = lstBook.FirstOrDefault(x => x.BookID == id);
            if (book == null)
            {
                return BadRequest("Sách không tồn tại.");
            }

            string json = Request.Cookies["lstComboItem"];
            List<ComboItem> myList = new List<ComboItem>();
            if (json != null)
            {
                myList = JsonConvert.DeserializeObject<List<ComboItem>>(json);
            }

            ComboItem cbi = new ComboItem();
            cbi.ComboItemID = Guid.NewGuid();
            cbi.BookID = book.BookID;
            cbi.ComboID = null;
            cbi.ItemName = book.BookName;
            cbi.Price = book.Price;
            cbi.Quantity = 1;
            cbi.ToTal = book.Price * cbi.Quantity;
            cbi.Status = 1;

            myList.Add(cbi);

            string updatedJson = JsonConvert.SerializeObject(myList);
            Response.Cookies.Append("lstComboItem", updatedJson);

            return RedirectToAction("CreateCombo", "ComboManager", new { area = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateCombo(Combo cb, IFormFile imageFile)
        {

            var urlBook = $"https://localhost:7079/api/Book/get-all-book";
            var httpClient = new HttpClient();
            var responseBook = await httpClient.GetAsync(urlBook);

            if (!responseBook.IsSuccessStatusCode)
            {
                return BadRequest("Lỗi khi tải danh sách sách.");
            }

            string apiDataBook = await responseBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            ViewBag.lstBook = lstBook;

            // Lấy danh sách ComboItem từ Cookie
            string json = Request.Cookies["lstComboItem"];
            if (json != null)
            {
                List<ComboItem> myList = JsonConvert.DeserializeObject<List<ComboItem>>(json);
                ViewBag.ListComboItem = myList;
                ComboItems = myList;
            }

            // Kiểm tra và lưu hình ảnh Combo
            if (imageFile != null && imageFile.Length > 0)
            {
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "images", imageFile.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                cb.Image = imageFile.FileName;
            }
            cb.ComboID = Guid.NewGuid();
            if (ComboItems == null)
            {
                return BadRequest("Danh sách ComboItem rỗng.");
            }
            


            // Lưu Combo vào cơ sở dữ liệu
            var urlCombo = $"https://localhost:7079/api/Combo/CreateCombo?comboname={cb.ComboName}&price={cb.Price}&status={cb.Status}&image={cb.Image}";
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonConvert.SerializeObject(cb), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(urlCombo, content);

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Thêm Thất Bại";
                return View();
            }
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                List<ComboItem> lst = ComboItems;
                bool allRequestsSuccessful = true;

                foreach (var item in lst)
                {
                    // Thiết lập ComboID cho từng ComboItem để nó trỏ đến Combo vừa tạo
                    ComboItem cbItem = new ComboItem
                    {
                        ComboItemID = Guid.NewGuid(),
                        BookID = item.BookID,
                        ComboID = cb.ComboID,
                        ItemName = item.ItemName,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        ToTal = item.ToTal,
                        Status = item.Status
                    };

                    // Lưu từng ComboItem vào cơ sở dữ liệu
                    var urlComboItemOfCombo = $"https://localhost:7079/api/ComboItem/Add-ComboItem?BookID={cbItem.BookID}&ComboID={cbItem.ComboID}&ItemName={cbItem.ItemName}&Price={cbItem.Price}&Quantity={cbItem.Quantity}&ToTal={cbItem.ToTal}&Status={cbItem.Status}";
                    var contentComboItemDetail = new StringContent(JsonConvert.SerializeObject(cbItem), Encoding.UTF8, "application/json");
                    var responseCBIT = await _httpClientz.PostAsync(urlComboItemOfCombo, contentComboItemDetail);

                    if (!responseCBIT.IsSuccessStatusCode)
                    {
                        allRequestsSuccessful = false;
                        break;
                    }
                }

                if (allRequestsSuccessful)
                {
                    // Nếu mọi thứ thành công, lưu thay đổi vào cơ sở dữ liệu
                    transactionScope.Complete();
                }
            }
            // Sau khi lưu Combo thành công, xóa Cookie chứa danh sách ComboItem
            Response.Cookies.Delete("lstComboItem");

            return RedirectToAction("AllComboManager", "ComboManager", new { area = "Admin" });
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

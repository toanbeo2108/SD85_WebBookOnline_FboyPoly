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
using System.Net;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using System.Transactions;
using SD85_WebBookOnline.Share.Data;

namespace SD85_WebBookOnline.Client.Areas.Employee.Controllers
{
    public class ComboEmployeeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClient _httpClientz;
        private readonly IWebHostEnvironment _webHostEnvironment;
        AppDbContext context;
        public List<ComboItem> ComboItems { get; set; } = new List<ComboItem>();

        public ComboEmployeeController(IWebHostEnvironment webHostEnvironment)
        {
            _httpClient = new HttpClient();
            _httpClientz = new HttpClient();
            _webHostEnvironment = webHostEnvironment;
            context = new AppDbContext();
        }



        [AutoValidateAntiforgeryToken]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AllComboEpl()
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
        public async Task<IActionResult> ComboDetailEpl(Guid id)
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

        public async Task<IActionResult> CreateComboEpl()
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

        public async Task<IActionResult> AddToComboEpl(Guid id)
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

            // Kiểm tra xem sách đã có trong danh sách ComboItem chưa
            var existingItem = myList.FirstOrDefault(x => x.BookID == book.BookID);
            if (existingItem != null)
            {
                // Nếu sách đã có, k tăng số lượng lên
                existingItem.Quantity += 0;
                existingItem.ToTal = existingItem.Price * existingItem.Quantity;
            }
            else
            {
                // Nếu sách chưa có, thêm mới vào danh sách
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
            }

            string updatedJson = JsonConvert.SerializeObject(myList);
            Response.Cookies.Append("lstComboItem", updatedJson);

            return RedirectToAction("CreateComboEpl", "ComboEmployee", new { area = "Employee" });
        }

        public async Task<IActionResult> DeleteToComboEpl(Guid id)
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
            List<ComboItem> myList = new List<ComboItem>();
            if (json != null)
            {
                myList = JsonConvert.DeserializeObject<List<ComboItem>>(json);
            }
            // Kiểm tra xem sách đã có trong danh sách ComboItem chưa
            var existingItem = myList.FirstOrDefault(x => x.ComboItemID == id);
            if (existingItem == null)
            {
                return BadRequest("Xóa sp ko thành công");
            }
            else
            {
                if (myList.Remove(existingItem))
                {
                    string updatedJson = JsonConvert.SerializeObject(myList);
                    Response.Cookies.Append("lstComboItem", updatedJson);
                    return RedirectToAction("CreateComboEpl", "ComboEmployee", new { area = "Employee" });
                }
                else
                {
                    return BadRequest("Hiep sai r nhes");
                }
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateComboEpl(Combo cb, IFormFile imageFile)
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
            if (ComboItems == null)
            {
                return BadRequest("Danh sách ComboItem rỗng.");
            }

            // Kiểm tra số lượng sách tồn có đủ để tạo combo không :
            List<ComboItem> myListCheck = JsonConvert.DeserializeObject<List<ComboItem>>(json);
            foreach (var item in myListCheck)
            {
                var UrlCheck = $"https://localhost:7079/api/Book/CheckQuantity?BookID={item.BookID}&Quantity={cb.Quantity}";
                var contentCheck = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                var responseCheck = await _httpClient.PostAsync(UrlCheck, contentCheck);
                if (responseCheck.IsSuccessStatusCode)
                {
                    string result = await responseCheck.Content.ReadAsStringAsync();
                    var KqCheck = JsonConvert.DeserializeObject<bool>(result);
                    if (KqCheck != true)
                    {
                        return BadRequest($"Số lượng sản phẩm {item.ItemName} không đáp ứng đủ");
                    }
                }
                else
                {
                    return BadRequest("Lỗi kiểm tra số lượng sản phẩm còn tồn trong cửa hàng");
                }
            }


            cb.ComboID = Guid.NewGuid();
            // Lưu Combo vào cơ sở dữ liệu
            var urlCombo = $"https://localhost:7079/api/Combo/CreateCombo?ComBoId={cb.ComboID}&comboname={cb.ComboName}&quanTity={cb.Quantity}&price={cb.Price}&status={cb.Status}&image={cb.Image}";
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
                        Quantity = 1,
                        ToTal = item.ToTal,
                        Status = item.Status
                    };


                    var urlComboItemOfCombo = $"https://localhost:7079/api/ComboItem/Add-ComboItem?BookID={cbItem.BookID}&ComboID={cbItem.ComboID}&ItemName={cbItem.ItemName}&Price={cbItem.Price}&Quantity={cbItem.Quantity}&ToTal={cbItem.ToTal}&Status={cbItem.Status}";
                    var contentComboItemDetail = new StringContent(JsonConvert.SerializeObject(cbItem), Encoding.UTF8, "application/json");
                    var responseCBIT = await _httpClient.PostAsync(urlComboItemOfCombo, contentComboItemDetail);

                    if (!responseCBIT.IsSuccessStatusCode)
                    {
                        allRequestsSuccessful = false;
                        break;
                    }

                    // Tạo xong comboItem thì chỉnh sửa lại số lượng tồn của sản phẩm
                    Book b = lstBook.FirstOrDefault(p => p.BookID == cbItem.BookID);
                    int CbQuantity = Convert.ToInt32(cb.Quantity);
                    b.QuantityExists -= CbQuantity;
                    var urlUpdateQuantity = $"https://localhost:7079/api/Book/UpdateQuantity?id={b.BookID}&TotalQuantity={b.TotalQuantity}&QuantitySold={b.QuantitySold}&QuantityExists={b.QuantityExists}";
                    var contentUpdateQuantity = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");
                    var responseUpdateQuantity = await _httpClient.PutAsync(urlUpdateQuantity, contentUpdateQuantity);


                }

                if (allRequestsSuccessful)
                {
                    // Nếu mọi thứ thành công, lưu thay đổi vào cơ sở dữ liệu
                    transactionScope.Complete();
                }
            }
            // Sau khi lưu Combo thành công, xóa Cookie chứa danh sách ComboItem
            Response.Cookies.Delete("lstComboItem");

            return RedirectToAction("AllComboEpl", "ComboEmployee", new { area = "Employee" });
        }
    }
}

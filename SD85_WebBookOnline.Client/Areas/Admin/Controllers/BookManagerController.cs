using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http.Headers;
using System.Text;

namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class BookManagerController : Controller
    {
        private HttpClient _httpClient;
        public BookManagerController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AllBookManager()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/Book/get-all-book";
            var httpClient = new HttpClient();
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            return View(lstBook);

        }
        [HttpGet]
        public IActionResult CreateBook()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateBook(Book bk,IFormFile imageFile)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            bk.BookID = Guid.NewGuid();
            //bk.CreateDate = DateTime.Now;
            if (imageFile != null && imageFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", imageFile.FileName);
                var stream = new FileStream(path, FileMode.Create);
                imageFile.CopyTo(stream);
                bk.MainPhoto = imageFile.FileName;
            }
            var urlBook = $"https://localhost:7079/api/Book/add-book?ManufacturerID={bk.ManufacturerID}&FormID={bk.FormID}&CouponID={bk.CouponID}&BookName={bk.BookName}&TotalQuantity={bk.TotalQuantity}&MainPhoto={bk.MainPhoto}&QuantitySold={bk.QuantitySold}&QuantityExists={bk.QuantityExists}&EntryPrice={bk.EntryPrice}&Price={bk.Price}&Information={bk.Information}&Description={bk.Description}&ISBN={bk.ISBN}&YearOfRelease={bk.YearOfRelease}&TransactionStatus={bk.TransactionStatus}&Status={bk.Status}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(urlBook, content);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("AllBookManager", "BookManager", new { area = "Admin" });
            }
            TempData["erro message"] = "thêm thất bại";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> BookDetail(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/Book/get-all-book";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.BookID == id);
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
        public async Task<IActionResult> UpdateBook(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/Book/get-all-book";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.BookID == id);
            if (Book == null)
            {
                return BadRequest();
            }
            else
            {
                return View(Book);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateBook(Guid id, Book vc)
        {
            var urlBook = $"https://localhost:7079/api/Book/updat-Book/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //return RedirectToAction("AllBookManager", "BookManager", new { area = "Admin" });
            return RedirectToAction("AllBookManager", "BookAdmin", new { area = "Admin" });

        }
        [HttpGet]
        public async Task<IActionResult> DeleteVoucher(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/Book/delete-Book/{id}";
            var respon = await _httpClient.DeleteAsync(urlBook);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            return RedirectToAction("AllBookManager", "BookManager", new { area = "Admin" });
        }
    }
}

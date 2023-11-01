using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Thêm thư viện để sử dụng IFormFile
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.IO; // Thêm thư viện để sử dụng FileStream và Path

namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class BookManagerController : Controller
    {
        private HttpClient _httpClient;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public BookManagerController(IWebHostEnvironment hostingEnvironment)
        {
            _httpClient = new HttpClient();
            _hostingEnvironment = hostingEnvironment;
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
        public async Task<IActionResult> CreateBook()
        {
            var urlManufacturer = $"https://localhost:7079/api/Manufacturer/GetAllManufacturer";
            var responManufacturer = await _httpClient.GetAsync(urlManufacturer);
            string apiDataManufacturer = await responManufacturer.Content.ReadAsStringAsync();
            var lstManufacturer = JsonConvert.DeserializeObject<List<Manufacturer>>(apiDataManufacturer);
            ViewBag.lstManufacturer = lstManufacturer;

            var urlCoupon = $"https://localhost:7079/api/Coupon/GetAllCoupon";
            var responCoupon = await _httpClient.GetAsync(urlCoupon);
            string apiDataCoupon = await responCoupon.Content.ReadAsStringAsync();
            var lstCoupon = JsonConvert.DeserializeObject<List<Coupon>>(apiDataCoupon);
            ViewBag.lstCoupon = lstCoupon;

            var urlForm = $"https://localhost:7079/api/Form/GetAllForm";
            var responForm = await _httpClient.GetAsync(urlForm);
            string apiDataForm = await responForm.Content.ReadAsStringAsync();
            var lstForm = JsonConvert.DeserializeObject<List<Form>>(apiDataForm);
            ViewBag.lstForm = lstForm;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateBook(Book bk, IFormFile imageFile)
        {
            var urlManufacturer = $"https://localhost:7079/api/Manufacturer/GetAllManufacturer";
            var responManufacturer = await _httpClient.GetAsync(urlManufacturer);
            string apiDataManufacturer = await responManufacturer.Content.ReadAsStringAsync();
            var lstManufacturer = JsonConvert.DeserializeObject<List<Manufacturer>>(apiDataManufacturer);
            ViewBag.lstManufacturer = lstManufacturer;

            var urlCoupon = $"https://localhost:7079/api/Coupon/GetAllCoupon";
            var responCoupon = await _httpClient.GetAsync(urlCoupon);
            string apiDataCoupon= await responCoupon.Content.ReadAsStringAsync();
            var lstCoupon = JsonConvert.DeserializeObject<List<Coupon>>(apiDataCoupon);
            ViewBag.lstCoupon = lstCoupon;

            

            var urlForm = $"https://localhost:7079/api/Form/GetAllForm";
            var responForm = await _httpClient.GetAsync(urlForm);
            string apiDataForm = await responForm.Content.ReadAsStringAsync();
            var lstForm = JsonConvert.DeserializeObject<List<Form>>(apiDataForm);
            ViewBag.lstForm = lstForm;


            if (imageFile != null && imageFile.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photoBooks", imageFile.FileName);
                var stream = new FileStream(path, FileMode.Create);
                imageFile.CopyTo(stream);
                bk.MainPhoto = imageFile.FileName;
            }
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            bk.BookID = Guid.NewGuid();
            //bk.CreateDate = DateTime.Now;

           
            var urlBook = $"https://localhost:7079/api/Book/add-book?ManufacturerID={bk.ManufacturerID}&FormID={bk.FormID}&CouponID={bk.CouponID}&BookName={bk.BookName}&TotalQuantity={bk.TotalQuantity}&MainPhoto={bk.MainPhoto}&QuantitySold={bk.QuantitySold}&QuantityExists={bk.QuantityExists}&EntryPrice={bk.EntryPrice}&Price={bk.Price}&Information={bk.Information}&Description={bk.Description}&ISBN={bk.ISBN}&YearOfRelease={bk.YearOfRelease}&TransactionStatus={bk.TransactionStatus}&Status={bk.Status}";
            var httpClient = new HttpClient();

            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PostAsync(urlBook, content);
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
        public async Task<IActionResult> UpdateBook(Guid id, Book vc, IFormFile imageFile)
        {
            var urlBook = $"https://localhost:7079/api/Book/updat-Book/{id}";
            if (id != vc.BookID)
            {
                return BadRequest();
            }
            // Kiểm tra xem tệp hình ảnh mới có được cung cấp hay không
            if (imageFile != null && imageFile.Length > 0)
            {
                // Xác định đường dẫn lưu trữ hình ảnh mới
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos", imageFile.FileName);

                // Lưu tệp hình ảnh mới
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Cập nhật thuộc tính 'MainPhoto' với tên tệp hình ảnh mới
                vc.MainPhoto = imageFile.FileName;
            }
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return RedirectToAction("AllBookManager", "BookManager", new { area = "Admin" });

        }
    

        [HttpPost]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            if (await TryDeleteBook(id))
            {
                return RedirectToAction("AllBookManager", "BookManager", new { area = "Admin" });
            }

            TempData["ErrorMessage"] = "Xóa Combo không thành công"; // Thêm thông báo lỗi
            return RedirectToAction("AllBookManager", "BookManager", new { area = "Admin" });
        }

        private async Task<bool> TryDeleteBook(Guid id)
        {
            try
            {
                var token = Request.Cookies["Token"];
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var urlBook = $"https://localhost:7079/api/Book/delete-book/{id}";

                var respon = await _httpClient.DeleteAsync(urlBook);

                return respon.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

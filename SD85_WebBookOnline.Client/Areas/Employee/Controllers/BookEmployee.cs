using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http.Headers;
using System.Text;

namespace SD85_WebBookOnline.Client.Areas.Employee.Controllers
{
    public class BookEmployee : Controller
    {
        bool _stt = false;
        string _mess = "";
        object _data = null;
        private HttpClient _httpClient;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public BookEmployee(IWebHostEnvironment hostingEnvironment)
        {
            _httpClient = new HttpClient();
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AllBookEmployee()
        {
            var urlCategory = $"https://localhost:7079/api/Category/GetAllCategory";
            var responCategory = await _httpClient.GetAsync(urlCategory);
            string apiDataCategory = await responCategory.Content.ReadAsStringAsync();
            var lstCategory = JsonConvert.DeserializeObject<List<Category>>(apiDataCategory);
            ViewBag.lstCategory = lstCategory;

            var urlAuthor = $"https://localhost:7079/api/Author/GetAllAuthor";
            var responAuthor = await _httpClient.GetAsync(urlAuthor);
            string apiAuthor = await responAuthor.Content.ReadAsStringAsync();
            var lstAuthor = JsonConvert.DeserializeObject<List<Author>>(apiAuthor);
            ViewBag.lstAuthor = lstAuthor;

            var urlLanguge = $"https://localhost:7079/api/Languge/GetAllLanguge";
            var responLanguge = await _httpClient.GetAsync(urlLanguge);
            string apiDataLanguge = await responLanguge.Content.ReadAsStringAsync();
            var lstLanguge = JsonConvert.DeserializeObject<List<Languge>>(apiDataLanguge);
            ViewBag.lstLanguge = lstLanguge;



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

            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/Book/get-all-book";
            var httpClient = new HttpClient();
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            ViewBag.lstBook = lstBook;
            return View(lstBook);
        }
        [HttpGet]
        public async Task<IActionResult> CreateBookEmployee()
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
        [HttpPost, Route("Add-Bookepl")]
        public async Task<IActionResult> CreateBookEmployee(Book bk, IFormFile imageFile)
        {
            var urlAllBook = $"https://localhost:7079/api/Book/get-all-book";
            var responAllBook = await _httpClient.GetAsync(urlAllBook);
            string apiDataAllBook = await responAllBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataAllBook);
            if (responAllBook.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (lstBook == null)
                {

                    _stt = false;
                    _mess = "Hiện Tại Không có sách trên Gian Hàng";
                }

            }

            var book = lstBook.FirstOrDefault(x => x.BookName.ToLower() == bk.BookName.ToLower());
            if (book == null)
            {
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


                var urlBook = $"https://localhost:7079/api/Book/add-book?bookid={bk.BookID}&ManufacturerID={bk.ManufacturerID}&FormID={bk.FormID}&CouponID={bk.CouponID}&BookName={bk.BookName}&TotalQuantity={bk.TotalQuantity}&MainPhoto={bk.MainPhoto}&QuantitySold={bk.QuantitySold}&QuantityExists={bk.QuantityExists}&EntryPrice={bk.EntryPrice}&Price={bk.Price}&Information={bk.Information}&Description={bk.Description}&ISBN={bk.ISBN}&YearOfRelease={bk.YearOfRelease}&weight={bk.Weight}&volume={bk.Volume}&TransactionStatus={bk.TransactionStatus}&Status={bk.Status}";
                var httpClient = new HttpClient();

                var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
                var respon = await _httpClient.PostAsync(urlBook, content);
                if (respon.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _stt = true;
                    _mess = "Thêm thành công!";

                }
                else
                {
                    _stt = false;
                    _mess = "Lỗi ! Thêm thất bại";
                }
            }
            else
            {
                _stt = false;
                _mess = "Sách này đã tồn tại !";
            }
            return Json(new
            {
                status = _stt,
                message = _mess,

            });

        }
        [HttpGet, Route("detail-bookepl/{id}")]
        public async Task<IActionResult> BookDetailEmployee(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/Book/get-all-book";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.BookID == id);



            if (responBook.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (Book == null)
                {
                    _stt = false;
                    _mess = "Không tìm thấy!";
                }
                else
                {
                    _stt = true;
                    _mess = "";
                    _data = Book;
                }
            }
            else
            {
                _stt = false;
                _mess = "Lỗi";
            }
            return Json(new
            {
                status = _stt,
                message = _mess,
                data = _data
            });


        }
        [HttpGet]
        public async Task<IActionResult> UpdateBookEmployee(Guid id)
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
        [HttpPost, Route("update-Bookelp/{id}")]
        public async Task<IActionResult> UpdateBookEmployee(Guid id, Book vc, IFormFile imageFile)
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
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photoBooks", imageFile.FileName);

                // Lưu tệp hình ảnh mới
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                // Cập nhật thuộc tính 'MainPhoto' với tên tệp hình ảnh mới
                vc.MainPhoto = imageFile.FileName;
            }
            else
            {
                var token = Request.Cookies["Token"];
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var url_Book = $"https://localhost:7079/api/Book/get-all-book";
                var responBook = await _httpClient.GetAsync(url_Book);
                string apiDataBook = await responBook.Content.ReadAsStringAsync();
                var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
                var Book = lstBook.FirstOrDefault(x => x.BookID == id);
                vc.MainPhoto = Book.MainPhoto + "";
            }
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            //if (!respon.IsSuccessStatusCode)
            //{
            //    return BadRequest();
            //}
            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //return RedirectToAction("AllBookManager", "BookManager", new { area = "Admin" });
            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
            {

                _stt = true;
                _mess = "cập nhật thành công !";
            }
            else
            {
                _stt = false;
                _mess = "thất bại!";
            }
            return Json(new
            {
                status = _stt,
                message = _mess
            });
        }


        [HttpPost, Route("Dell-Book/{id}")]
        public async Task<IActionResult> DeleteBook(Guid id)
        {
            //if (await TryDeleteBook(id))
            //{
            //    return RedirectToAction("AllBookManager", "BookManager", new { area = "Admin" });
            //}

            //TempData["ErrorMessage"] = "Xóa Combo không thành công"; // Thêm thông báo lỗi
            //return RedirectToAction("AllBookManager", "BookManager", new { area = "Admin" });
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/Book/delete-book/{id}";

            var respon = await _httpClient.DeleteAsync(urlBook);
            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _stt = true;
                _mess = "xóa thành công";
            }
            else
            {
                _stt = false;
                _mess = "xóa thất bại";
            }
            return Json(new
            {
                status = _stt,
                message = _mess,
            });
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

        [HttpPost, Route("UpdateStatusepl/{id}")]
        public async Task<IActionResult> UpdateBookStatusEmployee(Guid id, int newStatus)
        {
            try
            {
                var token = Request.Cookies["Token"];
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var urlUpdateStatus = $"https://localhost:7079/api/Book/UpdateStatus/{id}?newStatus={newStatus}";
                var response = await _httpClient.PutAsync(urlUpdateStatus, null);

                if (response.IsSuccessStatusCode)
                {
                    _stt = true;
                    _mess = "Cập nhật trạng thái thành công!";
                }
                else
                {
                    _stt = false;
                    _mess = "Cập nhật trạng thái thất bại!";
                }


            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                _stt = false;
                _mess = ex.Message;
            }
            return Json(new
            {
                status = _stt,
                message = _mess
            });
        }
    }
}

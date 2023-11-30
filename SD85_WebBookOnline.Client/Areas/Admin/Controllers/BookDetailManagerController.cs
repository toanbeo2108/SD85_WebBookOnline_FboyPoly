using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http.Headers;
using System.Text;

namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class BookDetailManagerController : Controller
    {
        string _mess = "";
        bool _stt = false;
        object _data = null;
        private HttpClient _httpClient;
        public BookDetailManagerController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        //https://localhost:7079/api/BookDetail/GetAllBookDetail
        //https://localhost:7079/api/BookDetail/CreateBookDetail
        //https://localhost:7079/api/BookDetail/UpdateBookDetail/{id}
        //https://localhost:7079/api/BookDetail/DeleteBookDetail/{id}

        [HttpGet]
        public async Task<IActionResult> AllBookDetailManager()
        {
            var urlBook = $"https://localhost:7079/api/Book/get-all-book";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            ViewBag.lstBook = lstBook;

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


            
            var urlBookDetail = $"https://localhost:7079/api/BookDetail/GetAllBookDetail";
            var responBookDetail = await _httpClient.GetAsync(urlBookDetail);
            var httpClient = new HttpClient();
           
            string apiDataBookDetail = await responBookDetail.Content.ReadAsStringAsync();
            var lstBookDetail = JsonConvert.DeserializeObject<List<BookDetail>>(apiDataBookDetail);
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            ViewBag.lstBookDetail = lstBookDetail;
            return View(lstBookDetail);
        }
        [HttpGet]
        public async Task<IActionResult> CreateBookDetail()
        {
            var urlBook = $"https://localhost:7079/api/Book/get-all-book";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            ViewBag.lstBook = lstBook;

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
            return View();
        }
        [HttpPost,Route("add-bookDetail")]
        public async Task<IActionResult> CreateBookDetail(BookDetail bk)
        {
            //var urlBook = $"https://localhost:7079/api/Book/get-all-book";
            //var responBook = await _httpClient.GetAsync(urlBook);
            //string apiDataBook = await responBook.Content.ReadAsStringAsync();
            //var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            //ViewBag.lstBook = lstBook;

            //var urlCategory = $"https://localhost:7079/api/Category/GetAllCategory";
            //var responCategory = await _httpClient.GetAsync(urlCategory);
            //string apiDataCategory = await responCategory.Content.ReadAsStringAsync();
            //var lstCategory = JsonConvert.DeserializeObject<List<Category>>(apiDataCategory);
            //ViewBag.lstCategory = lstCategory;

            //var urlAuthor = $"https://localhost:7079/api/Author/GetAllAuthor";
            //var responAuthor = await _httpClient.GetAsync(urlAuthor);
            //string apiAuthor = await responAuthor.Content.ReadAsStringAsync();
            //var lstAuthor = JsonConvert.DeserializeObject<List<Author>>(apiAuthor);
            //ViewBag.lstAuthor = lstAuthor;

            //var urlLanguge = $"https://localhost:7079/api/Languge/GetAllLanguge";
            //var responLanguge = await _httpClient.GetAsync(urlLanguge);
            //string apiDataLanguge = await responLanguge.Content.ReadAsStringAsync();
            //var lstLanguge = JsonConvert.DeserializeObject<List<Languge>>(apiDataLanguge);
            //ViewBag.lstLanguge = lstLanguge;


            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            bk.BookDetailID = Guid.NewGuid();
            //bk.CreateDate = DateTime.Now;
            var urlBookDetail = $"https://localhost:7079/api/BookDetail/CreateBookDetail?bookid={bk.BookID}&categoryid={bk.CategoriesID}&authorid={bk.AuthorID}&langugeid={bk.LagugeID}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PostAsync(urlBookDetail, content);
            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _stt = true;
                _mess = "Thêm thành công!";
            }
            else
            {
                _stt=false;
                _mess = "Thêm thất bại ";
            }
           
            return Json(new
            {

                status = _stt,
                message =_mess
            });
            
        }
        [HttpGet,Route("detail-bookdetail/{id}")]
        public async Task<IActionResult> BookDetailDetail(Guid id)
        {
            var urlBook = $"https://localhost:7079/api/Book/get-all-book";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            //var Book = lstBook.FirstOrDefault(x => x.BookID == id);
            ViewBag.lstBook = lstBook;

            var urlCategory = $"https://localhost:7079/api/Category/GetAllCategory";
            var responCategory = await _httpClient.GetAsync(urlCategory);
            string apiDataCategory = await responCategory.Content.ReadAsStringAsync();
            var lstCategory = JsonConvert.DeserializeObject<List<Category>>(apiDataCategory);
            //var Category = lstCategory.FirstOrDefault(x => x.CategoryID == id);
            ViewBag.lstCategory = lstCategory;

            var urlAuthor = $"https://localhost:7079/api/Author/GetAllAuthor";
            var responAuthor = await _httpClient.GetAsync(urlAuthor);
            string apiAuthor = await responAuthor.Content.ReadAsStringAsync();
            var lstAuthor = JsonConvert.DeserializeObject<List<Author>>(apiAuthor);
            //var Author = lstAuthor.FirstOrDefault(x => x.AuthorID == id);
            ViewBag.lstAuthor = lstAuthor;

            var urlLanguge = $"https://localhost:7079/api/Languge/GetAllLanguge";
            var responLanguge = await _httpClient.GetAsync(urlLanguge);
            string apiDataLanguge = await responLanguge.Content.ReadAsStringAsync();
            var lstLanguge = JsonConvert.DeserializeObject<List<Languge>>(apiDataLanguge);
            //var Languge = lstLanguge.FirstOrDefault(x => x.LangugeID == id);
            ViewBag.lstLanguge = lstLanguge;

            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBookDetail = $"https://localhost:7079/api/BookDetail/GetAllBookDetail";
            var responBookDetail = await _httpClient.GetAsync(urlBookDetail);
            string apiDataBookDetail = await responBookDetail.Content.ReadAsStringAsync();
           
            if (responBookDetail.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var lstBookDetail = JsonConvert.DeserializeObject<List<BookDetail>>(apiDataBookDetail);
                var BookDetail = lstBookDetail.FirstOrDefault(x => x.BookDetailID == id);
                ViewBag.BookDetail = BookDetail;
                if (BookDetail == null)
                {
                    _stt = false;

                    _mess = "Không tìm thấy thông tin";
                }
                else
                {
                    _stt = true;
                    _data = BookDetail;
                    _mess = "";
                
                }
            }
            return Json(new
            {
                status = _stt,
                message = _mess,
                data = _data,
            });
        }
        [HttpGet]
        public async Task<IActionResult> UpdateBookDetail(Guid id)
        {
            var urlBook = $"https://localhost:7079/api/Book/get-all-book";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            ViewBag.lstBook = lstBook;

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

            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBookDetail = $"https://localhost:7079/api/BookDetail/GetAllBookDetail";
            var responBookDetail = await _httpClient.GetAsync(urlBookDetail);
            string apiDataBookDetail = await responBookDetail.Content.ReadAsStringAsync();
            var lstBookDetail = JsonConvert.DeserializeObject<List<BookDetail>>(apiDataBookDetail);
            var BookDetail = lstBookDetail.FirstOrDefault(x => x.BookDetailID == id);
            ViewBag.BookDetail = BookDetail;
            if (BookDetail == null)
            {
                return BadRequest();
            }
            else
            {
                return View(BookDetail);
            }
        }
        
        [HttpPost,Route("UpdateBookDetail/{id}")]
        public async Task<IActionResult> UpdateBookDetail(Guid id, BookDetail vc)
        {
            var urlBook = $"https://localhost:7079/api/Book/get-all-book";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            ViewBag.lstBook = lstBook;

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

            var urlBookDetail = $"https://localhost:7079/api/BookDetail/UpdateBookDetail/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBookDetail, content);
            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _stt = true;
                _mess = "Cập nhật thành công!";
            }
            else
            {
                _stt = false;
                _mess = "Cập nhật thất bại ";
            }

            return Json(new
            {

                status = _stt,
                message = _mess
            });

        }
    }
}

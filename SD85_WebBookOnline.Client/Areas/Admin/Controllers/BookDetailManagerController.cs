using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http.Headers;
using System.Text;

namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class BookDetailManagerController : Controller
    {
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
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/BookDetail/GetAllBookDetail";
            var httpClient = new HttpClient();
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<BookDetail>>(apiDataBook);
            return View(lstBook);
        }
        [HttpGet]
        public IActionResult CreateBook()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateBookDetail(BookDetail bk)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            bk.BookDetailID = Guid.NewGuid();
            //bk.CreateDate = DateTime.Now;
            var urlBook = $"https://localhost:7079/api/BookDetail/CreateBookDetail?bookid={bk.BookID}&categoryid={bk.CategoriesID}&authorid={bk.AuthorID}&langugeid={bk.LagugeID}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(urlBook, content);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("AllBookDetailManager", "BookDetailManager", new { area = "Admin" });
            }
            TempData["erro message"] = "thêm thất bại";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> BookDetailDetail(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/BookDetail/GetAllBookDetail";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<BookDetail>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.BookDetailID == id);
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
        public async Task<IActionResult> UpdateBookDetail(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/BookDetail/GetAllBookDetail";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<BookDetail>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.BookDetailID == id);
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
        public async Task<IActionResult> UpdateBookDetail(Guid id, BookDetail vc)
        {
            var urlBook = $"https://localhost:7079/api/BookDetail/UpdateBookDetail/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return RedirectToAction("AllBookDetailManager", "BookDetailManager", new { area = "Admin" });

        }
    }
}

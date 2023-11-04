using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http.Headers;
using System.Text;

namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class CategoryManagerController : Controller
    {
        private HttpClient _httpClient;
        public CategoryManagerController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        //https://localhost:7079/api/Category/GetAllCategory
        //https://localhost:7079/api/Category/CreateCategory
        //https://localhost:7079/api/Category/UpdateCategory/{id}
        //https://localhost:7079/api/Category/DeleteCategory/{id}
        [HttpGet]
        public async Task<IActionResult> AllCategoryManager()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlForm = $"https://localhost:7079/api/Category/GetAllCategory";
            var httpClient = new HttpClient();
            var responForm = await _httpClient.GetAsync(urlForm);
            string apiDataForm = await responForm.Content.ReadAsStringAsync();
            var lstForm = JsonConvert.DeserializeObject<List<Category>>(apiDataForm);
            return View(lstForm);
        }
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category bk)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            bk.CategoryID = Guid.NewGuid();
            //bk.CreateDate = DateTime.Now;
            var urlBook = $"https://localhost:7079/api/Category/CreateCategory?name={bk.Name}&description={bk.Description}&status={bk.Status}";
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
        [HttpGet]
        public async Task<IActionResult> CategoryDetail(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/Category/GetAllCategory";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Category>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.CategoryID == id);
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
        public async Task<IActionResult> UpdateCategory(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/Category/GetAllCategory";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Category>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.CategoryID == id);
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
        public async Task<IActionResult> UpdateCategory(Guid id, Form vc)
        {
            var urlBook = $"https://localhost:7079/api/Category/UpdateCategory/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return RedirectToAction("AllFormManager", "FormManager", new { area = "Admin" });

        }
    }
}

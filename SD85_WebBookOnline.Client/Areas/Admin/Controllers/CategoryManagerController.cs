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
        bool _stt = false;
        string _mess = "";
        object _data = null;
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
        [HttpPost,Route("add-category")]
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
            //if (respon.IsSuccessStatusCode)
            //{
            //    return RedirectToAction("AllCategoryManager", "CategoryManager", new { area = "Admin" });
            //}
            //TempData["erro message"] = "thêm thất bại";
            //return View();
            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _stt = true;
                _mess = "Thêm thành công !";
                
            }
            else
            {
                _stt = false;
                _mess = "Thêm thất bại";
            }
            return Json(new
            {
                status = _stt,
                message = _mess,
            });
        }
        [HttpGet,Route("detail-category/{id}")]
        public async Task<IActionResult> CategoryDetail(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/Category/GetAllCategory";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Category>>(apiDataBook);
            var cart = lstBook.FirstOrDefault(x => x.CategoryID == id);
            //if (Book == null)
            //{
            //    return BadRequest();
            //}
            //else
            //{
            //    return View(Book);
            //}
            if (responBook.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _stt = true;
                _mess = "";
                _data = cart;
            }
            else
            {
                _stt = false;
                _mess = "Không tìm thấy thông tin";
            }
            return Json(new
            {
                status = _stt,
                message = _mess,
                data = _data
            });
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
        [HttpPost, Route("update-categorys/{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, Category vc)
        {
            var urlBook = $"https://localhost:7079/api/Category/UpdateCategory/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var respon = await _httpClient.PutAsync(urlBook, content);
            //if (!respon.IsSuccessStatusCode)
            //{
            //    return BadRequest();
            //}
            //return RedirectToAction("AllFormManager", "FormManager", new { area = "Admin" });
            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _stt = true;
                _mess = "Cập nhật thành công !";
            }
            else
            {
                _stt = false;
                _mess = "Cập nhật thất bại";
            }
            return Json(new
            {
                status = _stt,
                message = _mess,
            });
        }
    }
}

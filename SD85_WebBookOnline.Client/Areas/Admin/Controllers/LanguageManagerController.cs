using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http.Headers;
using System.Text;

namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class LanguageManagerController : Controller
    {
        private HttpClient _httpClient;
        string _mess = "";
        bool _stt = false;
        object _data = null;
        public LanguageManagerController()
        {
            _httpClient = new HttpClient();
        }
        public int pageSize = 6;
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AllLanguageManager(string txtSearch, int? page)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/Languge/GetAllLanguge";
            var httpClient = new HttpClient();
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstLanguage = JsonConvert.DeserializeObject<List<Languge>>(apiDataBook);

            if(lstLanguage == null)
            {
                return BadRequest();
            }

            var data = (from s in lstLanguage select s);
            if (!string.IsNullOrEmpty(txtSearch))
            {
                ViewBag.TxtSearch = txtSearch;
                data = data.Where(s => s.Name.Contains(txtSearch)).ToList();
            }

            if(page > 0)
            {
                page = page;
            }
            else
            {
                page = 1;
            }
            
            int start = (int)(page - 1) * pageSize;
            ViewBag.pageCurrent = page;
            int totalPage = data.Count();
            float totalNumberSize = (totalPage/(float)pageSize);
            int numSize = (int)Math.Ceiling(totalNumberSize);
            ViewBag.numSize = numSize;
            ViewBag.listLanguage = data.OrderByDescending(x => x.LangugeID).Skip(start).Take(pageSize);
            return View();
        }
        [HttpGet]
        public IActionResult CreateLanguage()
        {
            return View();
        }
        [HttpPost,Route("add-Language")]
        public async Task<IActionResult> CreateLanguage(Languge bk)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            bk.LangugeID = Guid.NewGuid();
            //bk.CreateDate = DateTime.Now;
            var urlBook = $"https://localhost:7079/api/Languge/CreateLanguge?name={bk.Name}&description={bk.Description}&status={bk.Status}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(urlBook, content);
            //if (respon.IsSuccessStatusCode)
            //{
            //    return RedirectToAction("AllLanguageManager", "LanguageManager", new { area = "Admin" });
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
        [HttpGet,Route("detail-Language/{id}")]
        public async Task<IActionResult> LanguageDetail(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/Languge/GetAllLanguge";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Languge>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.LangugeID == id);
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
                _data = Book;
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
        public async Task<IActionResult> UpdateLanguage(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/Languge/GetAllLanguge";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Languge>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.LangugeID == id);
            if (Book == null)
            {
                return BadRequest();
            }
            else
            {
                return View(Book);
            }
        }
        [HttpPost,Route("update-Languge/{id}")]
        public async Task<IActionResult> UpdateLanguage(Guid id, Languge vc)
        {
            var urlBook = $"https://localhost:7079/api/Languge/UpdateLaguge/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            //if (!respon.IsSuccessStatusCode)
                //{
                //    return BadRequest();
                //}
                var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //return RedirectToAction("AllLangugeManager", "LangugeManager", new { area = "Admin" });
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

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http.Headers;
using System.Text;

namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class FormManagerController : Controller
    {
        private HttpClient _httpClient;
        public FormManagerController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AllFormManager()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlForm = $"https://localhost:7079/api/Form/GetAllForm";
            var httpClient = new HttpClient();
            var responForm = await _httpClient.GetAsync(urlForm);
            string apiDataForm = await responForm.Content.ReadAsStringAsync();
            var lstForm = JsonConvert.DeserializeObject<List<Form>>(apiDataForm);
            return View(lstForm);
        }
        [HttpGet]
        public IActionResult CreateForm()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateForm(Form bk)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            bk.FormId = Guid.NewGuid();
            //bk.CreateDate = DateTime.Now;
            var urlBook = $"https://localhost:7079/api/Form/CreateForm?formName={bk.FormName}&description={bk.Description}&status={bk.Status}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(urlBook, content);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("MessageFormPageSucces", "FormManager", new { area = "Admin" });
            }
            ViewBag.MessageSucces = "Thêm thất bại";
            return RedirectToAction("MessageFormPageFalse", "FormManager", new { area = "Admin" });
        }
        [HttpGet]
        public async Task<IActionResult> FormDetail(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/Form/GetAllBill";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Form>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.FormId == id);
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
        public async Task<IActionResult> UpdateForm(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/Form/GetAllBill";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Form>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.FormId == id);
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
        public async Task<IActionResult> UpdateForm(Guid id, Form vc)
        {
            var urlBook = $"https://localhost:7079/api/Form/UpdateForm/{id}";
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
        public IActionResult MessageFormPageSucces()
        {
            return View();
        }
        public IActionResult MessageFormPageFalse()
        {
            return View();
        }
    }
}

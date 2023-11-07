using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http.Headers;
using System.Text;

namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class ManufacturerManagerController : Controller
    {
        private HttpClient _httpClient;
        public ManufacturerManagerController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AllManufacturerManager()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlManufacturer = $"https://localhost:7079/api/Manufacturer/GetAllManufacturer";
            var httpClient = new HttpClient();
            var responManufacturer = await _httpClient.GetAsync(urlManufacturer);
            string apiDataManufacturer = await responManufacturer.Content.ReadAsStringAsync();
            var lstManufacturer = JsonConvert.DeserializeObject<List<Manufacturer>>(apiDataManufacturer);
            return View(lstManufacturer);
        }
        [HttpGet]
        public IActionResult CreateManufacturer()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateManufacturer(Manufacturer bk)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            bk.ManufactureID = Guid.NewGuid();
            //bk.CreateDate = DateTime.Now;
            var urlBook = $"https://localhost:7079/api/Manufacturer/CreateManufacture?name={bk.ManufactureName}&description={bk.Desciption}&status={1}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(urlBook, content);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("AllManufacturerManager", "ManufacturerManager", new { area = "Admin" });
            }
            TempData["erro message"] = "thêm thất bại";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ManufacturerDetail(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/Manufacturer/GetAllManufacturer";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Manufacturer>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.ManufactureID == id);
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
        public async Task<IActionResult> UpdateManufacturer(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBook = $"https://localhost:7079/api/Manufacturer/GetAllManufacturer";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Manufacturer>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.ManufactureID == id);
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
        public async Task<IActionResult> UpdateManufacturer(Guid id, Manufacturer vc)
        {
            var urlBook = $"https://localhost:7079/api/Manufacturer/UpdateManufacturer/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlBook, content);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return RedirectToAction("AllManufacturerManager", "ManufacturerManager", new { area = "Admin" });

        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http.Headers;
using System.Text;

namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class ComboItemManagerController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ComboItemManagerController(IWebHostEnvironment webHostEnvironment)
        {
            _httpClient = new HttpClient();
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AllComboItemManager()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlComboItem = $"https://localhost:7079/api/ComboItem/GetAll-ComboItem";
            var httpClient = new HttpClient();
            var responComboItem = await _httpClient.GetAsync(urlComboItem);
            string apiDataComboItem = await responComboItem.Content.ReadAsStringAsync();
            var lstComboItem = JsonConvert.DeserializeObject<List<ComboItem>>(apiDataComboItem);
            return View(lstComboItem);
        }
        [HttpGet]
        public async Task<IActionResult> ComboItemDetail(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlComboItem = $"https://localhost:7079/api/ComboItem/GetAll-ComboItem";
            var responComboItem = await _httpClient.GetAsync(urlComboItem);
            string apiDataComboItem = await responComboItem.Content.ReadAsStringAsync();
            var lstComboItem = JsonConvert.DeserializeObject<List<ComboItem>>(apiDataComboItem);
            var comboitem = lstComboItem.FirstOrDefault(x => x.ComboItemID == id);
            if (comboitem == null)
            {
                return BadRequest();
            }
            else
            {
                return View(comboitem);
            }
        }


    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http;
using System.Text;

namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class ComboItemController : Controller
    {
        private readonly HttpClient _httpClient;
        public ComboItemController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllComboItem()
        {
            var urlComboItem = $"https://localhost:7079/api/ComboItem/GetAll-ComboItem";
            var responComboItem = await _httpClient.GetAsync(urlComboItem);
            string apiDataComboItem = await responComboItem.Content.ReadAsStringAsync();
            var lstComboItem = JsonConvert.DeserializeObject<List<ComboItem>>(apiDataComboItem);
            return View(lstComboItem);
        }
        public IActionResult CreateComboItem()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateComboItem(ComboItem cbi)
        {
            cbi.ComboID = Guid.Parse("7cb3b0aa-586b-490c-9005-89507f15fb9a");
            var urlComboItemOfCombo = $"https://localhost:7079/api/ComboItem/Add-ComboItem?BookID={cbi.BookID}&ComboID={cbi.ComboID}&ItemName={cbi.ItemName}&Price={cbi.Price}&Quantity={cbi.Quantity}&ToTal={cbi.ToTal}&Status={cbi.Status}";
            var contentComboItemDetail = new StringContent(JsonConvert.SerializeObject(cbi), Encoding.UTF8, "application/json");
            var responseCBIT = await _httpClient.PostAsync(urlComboItemOfCombo, contentComboItemDetail);

            if (!responseCBIT.IsSuccessStatusCode)
            {
                return BadRequest("Lỗi khi thêm ComboItem.");
            }
            return RedirectToAction("GetAllComboItem", "ComboItem", new {area = "Admin"});
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http.Headers;
using System.Text;

namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    //https://localhost:7079/api/Image/Update-image/937c4343-f9da-4f00-bef8-666c4a88c4ff
    //https://localhost:7079/api/Image/delete-image/ad931fd6-3d24-451c-a4a0-260d0f8ea7fc
    //
    //
    public class ImagesManagerController : Controller
    {
        HttpClient _httpClient;
        public ImagesManagerController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AllImages()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"https://localhost:7079/api/Image/getAll_Image";
            var httpClient = new HttpClient();
            var respon = await _httpClient.GetAsync(url);
            string apiData = await respon.Content.ReadAsStringAsync();
            var lst = JsonConvert.DeserializeObject<List<Images>>(apiData);
            ViewBag["b"] = new Book();
            return View(lst);
        }
        [HttpGet]
        public IActionResult CreateIMG()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateIMG(Images img)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            img.ImagesID = Guid.NewGuid();
            //bk.CreateDate = DateTime.Now;
            var url = $"https://localhost:7079/api/Image/add-image?BookID={img.BookID}&ImageName={img.ImageName}&Status={img.Status}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(img), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(url, content);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("AllImages", "ImagesManager", new { area = "Admin" });
            }
            TempData["erro message"] = "thêm thất bại";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ImagesDetail(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"https://localhost:7079/api/Image/getAll_Image";
            var respon = await _httpClient.GetAsync(url);
            string apiData = await respon.Content.ReadAsStringAsync();
            var lst = JsonConvert.DeserializeObject<List<Images>>(apiData);
            var IMGs = lst.FirstOrDefault(x => x.ImagesID == id);
            if (IMGs == null)
            {
                return BadRequest();
            }
            else
            {
                return View(IMGs);
            }
        }
        [HttpGet]
        public async Task<IActionResult> UpdateIMG(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"https://localhost:7079/api/Image/getAll_Image";
            var respon = await _httpClient.GetAsync(url);
            string apiData = await respon.Content.ReadAsStringAsync();
            var lst = JsonConvert.DeserializeObject<List<Images>>(apiData);
            var IMGs = lst.FirstOrDefault(x => x.ImagesID == id);
            if (IMGs == null)
            {
                return BadRequest();
            }
            else
            {
                return View(IMGs);
            }
        }
        [HttpPost]
         public async Task<IActionResult> UpdateIMG(Guid id, Images img)
        {
            var url = $"https://localhost:7079/api/Image/Update-image/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(img), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(url, content);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return RedirectToAction("AllImages", "ImagesManager", new { area = "Admin" });

        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"https://localhost:7079/api/Image/delete-image/{id}";
            var respon = await _httpClient.DeleteAsync(url);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            return RedirectToAction("AllImages", "ImagesManager", new { area = "Admin" });
        }   
    }
}

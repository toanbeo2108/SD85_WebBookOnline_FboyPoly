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
            var urlBook = $"https://localhost:7079/api/Book/get-all-book";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            ViewBag.lstBook = lstBook;

            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"https://localhost:7079/api/Image/getAll_Image";
            var httpClient = new HttpClient();
            var respon = await _httpClient.GetAsync(url);
            string apiData = await respon.Content.ReadAsStringAsync();
            var lst = JsonConvert.DeserializeObject<List<Images>>(apiData);
            //var ls = lst.Select(c => new
            //{
            //    a = c.Book.BookName,
            //    b = c.Status,

            //});

            //string qr = string.Format("...");

            return View(lst);
        }
        [HttpGet]
        public async Task<IActionResult> CreateIMG()
        {
           
            return View();
        }
        [HttpPost,Route("themm-image")]
        public async Task<IActionResult> CreateIMG(Images img)
        {
            string _mess = "";
            bool _stt = false;


           

            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            img.ImagesID = Guid.NewGuid();
            
            var url = $"https://localhost:7079/api/Image/add-image?BookID={img.BookID}&ImageName={img.ImageName}&Status={img.Status}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(img), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(url, content);
            //if (respon.IsSuccessStatusCode)
            //{

            //    return RedirectToAction("AllImages", "ImagesManager", new { area = "Admin" });

            //}

            //TempData["erro message"] = "thêm thất bại";
            //return View();
            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
            {
               
                _stt = true;
                _mess = "them thanh cong!";
                
            }
            else 
            {
                _stt = false;
                _mess = "them that bai!";
            }
            return Json(new
            {
                status = _stt,
                message = _mess
            });


        }
        [HttpGet, Route("detail-image/{id}")]
        public async Task<IActionResult> ImagesDetail(Guid id)
        {
            string _mess = "";
            bool _stt = false;
            object _data = null;

            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"https://localhost:7079/api/Image/getAll_Image";
            var respon = await _httpClient.GetAsync(url);
            string apiData = await respon.Content.ReadAsStringAsync();
            
            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var lst = JsonConvert.DeserializeObject<List<Images>>(apiData);
                var img = lst.FirstOrDefault(c => c.ImagesID == id);
                if (img == null)
                {
                    _stt = false;
                    _mess = "k tìm thấy";
                }
                else
                {
                    _stt = true;
                    _mess = "";
                    _data =  img ;
                }
                   
                                   
            }
            else
            {
                _stt= false;
                _mess = "không tìm thấy";
            }
            return Json(new
            {
                status = _stt,
                message = _mess,
                data = _data
            }) ;
            //string apiData = await respon.Content.ReadAsStringAsync();
            //var lst = JsonConvert.DeserializeObject<List<Images>>(apiData);
            //var IMGs = lst.FirstOrDefault(x => x.ImagesID == id);
            //if (IMGs == null)
            //{
            //    return BadRequest();
            //}
            //else
            //{
            //    return View(IMGs);
            //}
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
        [HttpPost,Route("update-img/{id}")]
         public async Task<IActionResult> UpdateIMG(Guid id, Images img)
        {
            string _mess = "";
            bool _stt = false;
           
            var url = $"https://localhost:7079/api/Image/Update-image/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(img), Encoding.UTF8, "application/json");
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var respon = await _httpClient.PutAsync(url, content);
            
            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
            {
                
                _stt = true;
                _mess = "cập nhật thành công !";
            }
            else
            {
                _stt = false;
                _mess = "thất bại!";
            }
            return Json(new
            {
                status = _stt,
                message = _mess
            });
            
            //if (!respon.IsSuccessStatusCode)
            //{
            //    return BadRequest();
            //}
           
            //return RedirectToAction("AllImages", "ImagesManager", new { area = "Admin" });

        }
        [HttpGet, Route("delete-image/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            string _mess = "";
            bool _stt = true;
            object _data = null;
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"https://localhost:7079/api/Image/delete-image/{id}";
            var respon = await _httpClient.DeleteAsync(url);
            //if (!respon.IsSuccessStatusCode)
            //{
            //    return BadRequest();
            //}
            //return RedirectToAction("AllImages", "ImagesManager", new { area = "Admin" });
            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _stt = true;
                _mess = "xóa thành công";
            }
            else
            {
                _stt = false;
                _mess = "xóa thất bại";
            }
            return Json(new
            {
                status = _stt,
                message = _mess,
            });

        }
        
    }
} 

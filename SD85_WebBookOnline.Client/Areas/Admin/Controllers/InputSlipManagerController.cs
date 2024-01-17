using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http.Headers;
using System.Text;
namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class InputSlipManagerController : Controller
    {
        HttpClient _httpClient;
        public InputSlipManagerController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AllInput()
        {
            var urlBook = $"https://localhost:7079/api/Book/get-all-book";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            ViewBag.lstBook = lstBook;

            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urluser = $"https://localhost:7079/api/user/GetAllUser";
            var response = await _httpClient.GetAsync(urluser);
            string apiDataUser = await response.Content.ReadAsStringAsync();
            var ListUser = JsonConvert.DeserializeObject<List<User>>(apiDataUser);
            ViewBag.listUser = ListUser;

            var url = $"https://localhost:7079/api/InputSlipController/GetAllInputSlip";
            var httpClient = new HttpClient();
            var respon = await _httpClient.GetAsync(url);
            string apiData = await respon.Content.ReadAsStringAsync();
            var lst = JsonConvert.DeserializeObject<List<InputSlip>>(apiData);


            return View(lst);
        }
        public async Task<IActionResult> CreateIP()
        {

            return View();
        }
        [HttpPost, Route("themm-inputslip")]
        public async Task<IActionResult> CreateIP(InputSlip ip)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            string _mess = "";
            bool _stt = false;
            string UserId = Request.Cookies["UserID"];


            ip.InputSlipID = Guid.NewGuid();
            ip.IdNhanVienNhap = UserId;
            ip.NgayNhap = DateTime.Now;
            var url = $"https://localhost:7079/api/InputSlipController/CreateInputSlip?idSachNhap={ip.IdSachNhap}&giaban={ip.GiaBan}&idNhanVienNhap={UserId}&soLuong={ip.SoLuong}&ngayNhap={ip.NgayNhap}&giaNhap={ip.GiaNhap}";
            // var url = $"https://localhost:7079/api/InputSlipController/CreateInputSlip?idSachNhap={ip.IdSachNhap}&soLuong={ip.SoLuong}&ngayNhap={ip.NgayNhap}&giaNhap={ip.GiaNhap}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(ip), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(url, content);
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
        [HttpGet, Route("detail-inp/{id}")]
        public async Task<IActionResult> InPDetail(Guid id)
        {
            string _mess = "";
            bool _stt = false;
            object _data = null;

            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"https://localhost:7079/api/InputSlipController/GetAllInputSlip";
            var respon = await _httpClient.GetAsync(url);
            string apiData = await respon.Content.ReadAsStringAsync();

            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var lst = JsonConvert.DeserializeObject<List<InputSlip>>(apiData);
                var img = lst.FirstOrDefault(c => c.InputSlipID == id);
                if (img == null)
                {
                    _stt = false;
                    _mess = "k tìm thấy";
                }
                else
                {
                    _stt = true;
                    _mess = "";
                    _data = img;
                }


            }
            else
            {
                _stt = false;
                _mess = "không tìm thấy";
            }
            return Json(new
            {
                status = _stt,
                message = _mess,
                data = _data
            });

        }
    }
}

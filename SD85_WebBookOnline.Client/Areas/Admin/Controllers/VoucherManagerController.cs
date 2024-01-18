using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Client.Models;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http.Headers;
using System.Text;

namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class VoucherManagerController : Controller
    {
        string _mess;
        bool _stt;
        object _data = null;
        private  HttpClient _httpClient;
        public VoucherManagerController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> AllVoucherManager()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlVoucher = $"https://localhost:7079/api/Voucher/GetAllVoucher";
            var httpClient = new HttpClient();
            var responVoucher = await _httpClient.GetAsync(urlVoucher);
            string apiDataVoucher = await responVoucher.Content.ReadAsStringAsync();
            var lstVoucher = JsonConvert.DeserializeObject<List<Voucher>>(apiDataVoucher);
            ViewBag.lstVoucher = lstVoucher;

            return View(lstVoucher);
        }
        public IActionResult CreateVoucher()
        {
            return View();
        }
        [HttpPost,Route("Add-Voucher")]
        public async Task<IActionResult> CreateVoucher(Voucher vc)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            vc.VoucherID = Guid.NewGuid();
            vc.Status = 1;
            var urlVoucher = $"https://localhost:7079/api/Voucher/CreateVoucher?name={vc.Name}&quantity={vc.Quantity}&code={vc.code}&description={vc.Description}&starDate={vc.StartDate}&endDate={vc.EndDate}&discountCondition={vc.DiscountCondition}&discountAmount={vc.DiscountAmount}&status={vc.Status}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8,"application/json");
            var respon = await httpClient.PostAsync(urlVoucher, content);
            //if (respon.IsSuccessStatusCode)
            //{
            //   return RedirectToAction("AllVoucherManager", "VoucherManager",new {area = "Admin"});
            //}
            //TempData["Erro Message"] = "Thêm Thất Bại";
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
        [HttpGet,Route("Voucher-Detail/{id}")]
        public async Task<IActionResult> VoucherDetail(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlVoucher = $"https://localhost:7079/api/Voucher/GetAllVoucher";
            var responVoucher = await _httpClient.GetAsync(urlVoucher);
            string apiDataVoucher = await responVoucher.Content.ReadAsStringAsync();
            var lstVoucher = JsonConvert.DeserializeObject<List<Voucher>>(apiDataVoucher);
            var voucher = lstVoucher.FirstOrDefault(x => x.VoucherID == id);
            //if(voucher == null)
            //{
            //    return BadRequest();
            //}
            //else
            //{
            //    return View(voucher);   
            //}
            if (responVoucher.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (voucher== null)
                {
                    _stt = false;
                    _mess = "Không tìn thấy";
                }
                else
                {
                    _stt = true;
                    _mess = "";
                    _data = voucher;
                }
               

            }
            else
            {
                _stt = false;
                _mess = responVoucher.ReasonPhrase + "";
            }
            return Json(new
            {
                status = _stt,
                message = _mess,
                data=_data
            });
        }
        [HttpGet]
        public async Task<IActionResult> UpdateVoucher(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlVoucher = $"https://localhost:7079/api/Voucher/GetAllVoucher";
            var responVoucher = await _httpClient.GetAsync(urlVoucher);
            string apiDataVoucher = await responVoucher.Content.ReadAsStringAsync();
            var lstVoucher = JsonConvert.DeserializeObject<List<Voucher>>(apiDataVoucher);
            var voucher = lstVoucher.FirstOrDefault(x => x.VoucherID == id);
            if (voucher == null)
            {
                return BadRequest();
            }
            else
            {
                return View(voucher);
            }
        }
        [HttpPost,Route("Update-Voucher/{id}")]
        public async Task<IActionResult> UpdateVoucher(Guid id,Voucher vc)
        {
            var urlVoucher = $"https://localhost:7079/api/Voucher/UpdateVoucher/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon =  await _httpClient.PutAsync(urlVoucher, content);
            //if (!respon.IsSuccessStatusCode)
            //{
            //    return BadRequest();
            //}
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //return RedirectToAction("AllVoucherManager", "VoucherAdmin", new { area = "Admin" });
            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
            {

                _stt = true;
                _mess = "Cập nhật thanh cong!";

            }
            else
            {
                _stt = false;
                _mess = "Cập nhật that bai!";
            }
            return Json(new
            {
                status = _stt,
                message = _mess
            });
        }
        [HttpGet,Route("Xoa_Voucher/{id}")]
        public async Task<IActionResult> DeleteVoucher(Guid id,Voucher vc)
        {
            var urlVoucher = $"https://localhost:7079/api/Voucher/DeleteVoucher/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon = await _httpClient.PutAsync(urlVoucher, content);
            //if (!respon.IsSuccessStatusCode)
            //{
            //    return BadRequest();
            //}
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //if (!respon.IsSuccessStatusCode)
            //{
            //    return BadRequest();
            //}
            //return RedirectToAction("AllVoucherManager", "VoucherAdmin", new { area = "Admin" });
            if (respon.StatusCode == System.Net.HttpStatusCode.OK)
            {

                _stt = true;
                _mess = "Xóa thanh cong!";

            }
            else
            {
                _stt = false;
                _mess = "Xóa that bai!";
            }
            return Json(new
            {
                status = _stt,
                message = _mess
            });
        }


        [HttpPost, Route("load-data-voucher")]
        public async Task<IActionResult> LOADDATAVOUCHER(filterOption filter)
        {

            bool _status = false;
            string _message = "";
            object _data = null;
            try
            {

                var token = Request.Cookies["Token"];
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var urlVoucher = $"https://localhost:7079/api/Voucher/GetAllVoucher";
                var httpClient = new HttpClient();
                var responVoucher = await _httpClient.GetAsync(urlVoucher);
                if (responVoucher.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string apiDataVoucher = await responVoucher.Content.ReadAsStringAsync();
                    var lstVoucher = JsonConvert.DeserializeObject<List<Voucher>>(apiDataVoucher);


                    if (filter != null)
                    {
                        if (filter.NTTU_tu != null)
                        {
                            lstVoucher = lstVoucher.Where(c => c.StartDate >= filter.NTTU_tu.Value).ToList();

                        }
                        if (filter.NTTU_den != null)
                        {
                            lstVoucher = lstVoucher.Where(c => c.StartDate <= filter.NTTU_den.Value).ToList();

                        }
                        if (filter.NTDEN_tu != null)
                        {
                            lstVoucher = lstVoucher.Where(c => c.EndDate >= filter.NTDEN_tu.Value).ToList();

                        }
                        if (filter.NTDEN_den != null)
                        {
                            lstVoucher = lstVoucher.Where(c => c.EndDate <= filter.NTDEN_den.Value).ToList();

                        }

                        if (filter.Status != null)
                        {
                            lstVoucher = lstVoucher.Where(c => c.Status <= filter.Status.Value).ToList();

                        }
                        if (filter._search != null)
                        {
                            lstVoucher = lstVoucher.Where(c => c.Name.ToLower() == filter._search.ToLower().ToString()).ToList();
                        }

                    }

                    _status = true;
                    _message = "lỌC DỮ LIỆU THÀNH CÔNG";
                    _data = lstVoucher;

                }
                else if (responVoucher.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    _status = false;
                    _message = "call api error badrequest";

                }
                else
                {
                    _status = false;
                    _message = responVoucher.ReasonPhrase + "";
                }



            }
            catch (Exception ex)
            {
                _status = false;
                _message = ex.Message;


            }
            return Json(new
            {
                status = _status,
                message = _message,
                data = _data
            });

        }
    }
}

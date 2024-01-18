using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http.Headers;
using System.Text;

namespace SD85_WebBookOnline.Client.Areas.Employee.Controllers
{
    public class BillEmployeeController : Controller
    {
        private HttpClient _httpClient;
        public BillEmployeeController()
        {
            _httpClient = new HttpClient();
        }
        public async Task<IActionResult> GetAllBill_epl()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var Url = $"https://localhost:7079/api/Bill/GetAllBill";
            var respone = await _httpClient.GetAsync(Url);
            string apiData = await respone.Content.ReadAsStringAsync();
            var Bills = JsonConvert.DeserializeObject<List<Bill>>(apiData);

            var UrlUser = $"https://localhost:7079/api/User/GetAllUser";
            var responeUser = await _httpClient.GetAsync(UrlUser);
            string apiDataUser = await responeUser.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<User>>(apiDataUser);
            ViewBag.Users = users;

            var Voucher_code = Request.Cookies["Voucher_id"];
            if (Voucher_code != null)
            {
                var UrlVoucher = $"https://localhost:7079/api/Voucher/GetVoucherByVoucherCode?VoucherCode={Voucher_code}";
                var responeVoucher = await _httpClient.GetAsync(UrlVoucher);
                string apiDataVoucher = await responeVoucher.Content.ReadAsStringAsync();
                var voucher = JsonConvert.DeserializeObject<Voucher>(apiDataVoucher);
                ViewBag.Voucher = voucher;
            }

            return View(Bills);
        }

        public async Task<IActionResult> DetailBill_epl(Guid id)
        {
            var UserId = Request.Cookies["UserID"];
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlUser = $"https://localhost:7079/api/user/GetUsersById?id=" + UserId;
            var responeUserl = await _httpClient.GetAsync(urlUser);
            string apiUser = await responeUserl.Content.ReadAsStringAsync();
            var User = JsonConvert.DeserializeObject<User>(apiUser);

            var url = $"https://localhost:7079/api/BillItem/GetAllBillItemByBillID/" + id;
            var respone = await _httpClient.GetAsync(url);
            string apiData = await respone.Content.ReadAsStringAsync();
            var ListBillItems = JsonConvert.DeserializeObject<List<BillItems>>(apiData);

            var urlBill = $"https://localhost:7079/api/Bill/GetBillByBillId/" + id;
            var responeBill = await _httpClient.GetAsync(urlBill);
            string apiBill = await responeBill.Content.ReadAsStringAsync();
            var Bill = JsonConvert.DeserializeObject<Bill>(apiBill);
            ViewBag.Bill = Bill;
            ViewBag.User = User;
            return View(ListBillItems);
        }
        public async Task<IActionResult> XacNhanBill_epl(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBill = $"https://localhost:7079/api/Bill/GetBillByBillId/" + id;
            var responeBill = await _httpClient.GetAsync(urlBill);
            string apiBill = await responeBill.Content.ReadAsStringAsync();
            var Bill = JsonConvert.DeserializeObject<Bill>(apiBill);
            Bill.Status = 2; // chuyển sang trạng thái đang giao

            var urlUpdateBill = $"https://localhost:7079/api/Bill/UpdateBill/" + Bill.BillID;
            var content = new StringContent(JsonConvert.SerializeObject(Bill), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(urlUpdateBill, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllBill_admin");
            }
            else
            {
                return BadRequest();
            }
        }
        public async Task<IActionResult> HoanthanhBill_epl(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBill = $"https://localhost:7079/api/Bill/GetBillByBillId/" + id;
            var responeBill = await _httpClient.GetAsync(urlBill);
            string apiBill = await responeBill.Content.ReadAsStringAsync();
            var Bill = JsonConvert.DeserializeObject<Bill>(apiBill);
            Bill.Status = 3; // chuyển sang trạng thái hoàn thành bill

            var urlUpdateBill = $"https://localhost:7079/api/Bill/UpdateBill/" + Bill.BillID;
            var content = new StringContent(JsonConvert.SerializeObject(Bill), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(urlUpdateBill, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllBill_admin");
            }
            else
            {
                return BadRequest();
            }
        }
        //public async Task<IActionResult> YeucauHuyBill_epl(Guid id)
        //{
        //    var token = Request.Cookies["Token"];
        //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    var urlBill = $"https://localhost:7079/api/Bill/GetBillByBillId/" + id;
        //    var responeBill = await _httpClient.GetAsync(urlBill);
        //    string apiBill = await responeBill.Content.ReadAsStringAsync();
        //    var Bill = JsonConvert.DeserializeObject<Bill>(apiBill);
        //    Bill.Status = 4; // chuyển sang trạng thái yêu cầu hủy

        //    var urlUpdateBill = $"https://localhost:7079/api/Bill/UpdateBill/" + Bill.BillID;
        //    var content = new StringContent(JsonConvert.SerializeObject(Bill), Encoding.UTF8, "application/json");
        //    var response = await _httpClient.PutAsync(urlUpdateBill, content);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        return RedirectToAction("GetAllBill_admin");
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}
        public async Task<IActionResult> XacNhanHuyBill_epl(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBill = $"https://localhost:7079/api/Bill/GetBillByBillId/" + id;
            var responeBill = await _httpClient.GetAsync(urlBill);
            string apiBill = await responeBill.Content.ReadAsStringAsync();
            var Bill = JsonConvert.DeserializeObject<Bill>(apiBill);
            Bill.Status = 0; // chuyển sang trạng thái hủy billl

            // Cập nhật lại số lượng sản phẩm trong giỏ hàng

            var urlUpdateBill = $"https://localhost:7079/api/Bill/UpdateBill/" + Bill.BillID;
            var content = new StringContent(JsonConvert.SerializeObject(Bill), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(urlUpdateBill, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("GetAllBill_admin");
            }
            else
            {
                return BadRequest();
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SD85_WebBookOnline.Client.Areas.Customer.Controllers
{
    public class BillController : Controller
    {
        private readonly HttpClient _httpClient;
        public BillController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IActionResult> GetAllBill()
        {
            var UserId = Request.Cookies["UserID"];
            var url = $"https://localhost:7079/api/Bill/GetAllBillByUserID?UserID={UserId}";
            var respone = await _httpClient.GetAsync(url);
            string apiData = await respone.Content.ReadAsStringAsync();
            var lstBill = JsonConvert.DeserializeObject<List<Bill>>(apiData);
            return View(lstBill);
        }
        [HttpGet]
        public async Task<IActionResult> GetBillDetails(Guid id)
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
    }
}

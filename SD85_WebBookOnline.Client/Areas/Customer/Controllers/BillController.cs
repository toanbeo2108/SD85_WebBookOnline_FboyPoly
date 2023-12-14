using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http;

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

            foreach (var item in lstBill)
            {
                var urlBillItems = $"https://localhost:7079/api/BillItem/GetAllBillItemByBillID/{item.BillID}";
                var responeBillItems = await _httpClient.GetAsync(urlBillItems);
                string apiDataBillItems = await responeBillItems.Content.ReadAsStringAsync();
                var lstBillItems = JsonConvert.DeserializeObject<List<BillItems>>(apiDataBillItems);

                ViewBag.ListBillItem = lstBillItems;
            }

            return View(lstBill);
        }
    }
}

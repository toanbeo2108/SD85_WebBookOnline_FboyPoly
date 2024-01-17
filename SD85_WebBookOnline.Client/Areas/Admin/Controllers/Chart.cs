using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Client.Models;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http.Headers;
using System.IO;
namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class RevenueByMonth
    {
        public int Month { get; set; }
        public decimal TotalRevenue { get; set; }
    }
    public class Chart : Controller
    {
        string _mess;
        bool _stt;
        object _data = null;
        HttpClient _httpClient;
        public Chart()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet, Route("GetChart")]
        public async Task<IActionResult> GetChart(int? selectedYear, int? selectedMonth)
        {
            try
            {
                var urlBill = "https://localhost:7079/api/Bill/GetAllBill";
                var urlBillitem = "https://localhost:7079/api/BillItem/GetAllBillItem";

                var billResponse = await _httpClient.GetAsync(urlBill);
                var billItemResponse = await _httpClient.GetAsync(urlBillitem);

                billResponse.EnsureSuccessStatusCode();
                billItemResponse.EnsureSuccessStatusCode();

                var billContent = await billResponse.Content.ReadAsStringAsync();
                var billItemContent = await billItemResponse.Content.ReadAsStringAsync();

                var bills = JsonConvert.DeserializeObject<IEnumerable<Bill>>(billContent);
                var billItems = JsonConvert.DeserializeObject<IEnumerable<BillItems>>(billItemContent);

                if (selectedYear.HasValue)
                {
                    bills = bills
                        .Where(b => b.OrderDate != null && b.OrderDate.Value.Year == selectedYear.Value && b.Status == 3 );
                }

                if (selectedMonth.HasValue)
                {
                    bills = bills
                        .Where(b => b.OrderDate != null && b.OrderDate.Value.Month == selectedMonth.Value && b.Status == 3);
                }

                var monthlyTotalRevenue = selectedMonth.HasValue
     ? Enumerable.Range(1, DateTime.DaysInMonth(selectedYear.Value, selectedMonth.Value))
         .Select(day =>
         {
             var totalRevenue = bills
                 .Where(b => b.OrderDate != null && b.OrderDate.Value.Day == day)
                 .Join(
                     billItems.Where(bi => bi.BookID != null), // Thêm điều kiện BookId != null vào đây
                     bill => bill.BillID,
                     billItem => billItem.BillID,
                     (bill, billItem) => billItem.ToTal)

                 .DefaultIfEmpty(0)
                 .Sum();

             return new RevenueByMonth { Month = day, TotalRevenue = totalRevenue };
         })
         .ToList()
     : Enumerable.Range(1, 12)
         .Select(month =>
         {
             var totalRevenue = bills
                 .Where(b => b.OrderDate != null && b.OrderDate.Value.Month == month)
                 .Join(
                     billItems.Where(bi => bi.BookID != null), // Thêm điều kiện BookId != null vào đây
                     bill => bill.BillID,
                     billItem => billItem.BillID,
                     (bill, billItem) => billItem.ToTal)
                 .DefaultIfEmpty(0)
                 .Sum();

             return new RevenueByMonth { Month = month, TotalRevenue = totalRevenue };
         })
         .ToList();


                _data = monthlyTotalRevenue;
                _mess = "Success";
                _stt = true;
            }
            catch (Exception ex)
            {
                _mess = ex.Message;
                _stt = false;
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

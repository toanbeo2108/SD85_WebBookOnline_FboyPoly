//using ClosedXML.Excel;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Client.Models;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http.Headers;
using System.IO;


namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class ThongKeManagerController : Controller
    {
        string _mess;
        bool _stt;
        object _data = null;
        HttpClient _httpClient;

        public ThongKeManagerController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
         [HttpGet, Route("GetThongKe")]
        public async Task<IActionResult> GetThongKe()
        {


            var urlBill = "https://localhost:7079/api/Bill/GetAllBill";
            var urlBook = "https://localhost:7079/api/Book/get-all-book";
            var urlBillitem = "https://localhost:7079/api/BillItem/GetAllBillItem";
            var urlInput = $"https://localhost:7079/api/InputSlipController/GetAllInputSlip";

            var BillksResponse = await _httpClient.GetAsync(urlBill);
            var booksResponse = await _httpClient.GetAsync(urlBook);
            var billItemsResponse = await _httpClient.GetAsync(urlBillitem);
            var InputResponse = await _httpClient.GetAsync(urlInput);

            BillksResponse.EnsureSuccessStatusCode();
            booksResponse.EnsureSuccessStatusCode();
            billItemsResponse.EnsureSuccessStatusCode();
            InputResponse.EnsureSuccessStatusCode();

            var billContent = await BillksResponse.Content.ReadAsStringAsync();
            var booksContent = await booksResponse.Content.ReadAsStringAsync();
            var billItemsContent = await billItemsResponse.Content.ReadAsStringAsync();
            var InputContent = await InputResponse.Content.ReadAsStringAsync();

            var books = JsonConvert.DeserializeObject<IEnumerable<Book>>(booksContent);
            var billItems = JsonConvert.DeserializeObject<IEnumerable<BillItems>>(billItemsContent);
            var Bills = JsonConvert.DeserializeObject<IEnumerable<Bill>>(billContent);
            var Input = JsonConvert.DeserializeObject<IEnumerable<InputSlip>>(InputContent);

            // Thống kê cho từng sách

            var thongKeData = new List<ThongKeViewModel>(); // Sử dụng List thay vì IEnumerable

            thongKeData.AddRange(from bi in billItems
                                 join b in books on bi.BookID equals b.BookID
                                 where bi.Status == 3
                                 group new { bi, b } by new { bi.BookID, b.BookName, b.QuantityExists } into g
                                 select new ThongKeViewModel
                                 {
                                     tensach = g.Key.BookName,
                                     TongSoSachBanDuoc = g.Sum(x => x.bi.Quantity),
                                     SoSachConLai = g.Key.QuantityExists,
                                     TongDoanhThusach = g.Sum(x => x.bi.Price * x.bi.Quantity),
                                     LoiNhuansach = g.Sum(x => (x.bi.Price - x.bi.GiaNhap) * x.bi.Quantity),
                                     ChiPhiGocsach = g.Sum(x => x.bi.GiaNhap * x.bi.Quantity)
                                 });

            var tongTongSoSachBanDuoctrongNgay = thongKeData.Sum(c => c.TongSoSachBanDuoc);
            var tongTongdoanhThutrongNgay = thongKeData.Sum(c => c.TongDoanhThusach);
            var tongTongLoiNhuantrongNgay = thongKeData.Sum(c => c.LoiNhuansach);
            var tongTongChiPhigoctrongNgay = thongKeData.Sum(c => c.ChiPhiGocsach);

            thongKeData.Add(new ThongKeViewModel
            {
                tensach = "Tổng cộng",
                TongSoSachBanDuoc = tongTongSoSachBanDuoctrongNgay,
                TongDoanhThusach = tongTongdoanhThutrongNgay,
                LoiNhuansach = tongTongLoiNhuantrongNgay,
                ChiPhiGocsach = tongTongChiPhigoctrongNgay,
            });

            return Json(new
            {
                status = true,
                message = _mess,
                data = thongKeData
            }); 
        }
        //  thống kê trong ngày hôm nay
        [HttpGet, Route("ThongkeNgay")]
        public async Task<IActionResult> GetThongKeNgayHomNay()
        {



            DateTime ngayHomNay = DateTime.Today;

            // Gọi API để lấy danh sách Bill và BillItems

            var urlBill = "https://localhost:7079/api/Bill/GetAllBill";
            var urlBook = "https://localhost:7079/api/Book/get-all-book";
            var urlBillitem = "https://localhost:7079/api/BillItem/GetAllBillItem";
            var urlInput = $"https://localhost:7079/api/InputSlipController/GetAllInputSlip";

            var BillksResponse = await _httpClient.GetAsync(urlBill);
            var booksResponse = await _httpClient.GetAsync(urlBook);
            var billItemsResponse = await _httpClient.GetAsync(urlBillitem);
            var InputResponse = await _httpClient.GetAsync(urlInput);

            BillksResponse.EnsureSuccessStatusCode();
            booksResponse.EnsureSuccessStatusCode();
            billItemsResponse.EnsureSuccessStatusCode();
            InputResponse.EnsureSuccessStatusCode();

            var billContent = await BillksResponse.Content.ReadAsStringAsync();
            var booksContent = await booksResponse.Content.ReadAsStringAsync();
            var billItemsContent = await billItemsResponse.Content.ReadAsStringAsync();
            var InputContent = await InputResponse.Content.ReadAsStringAsync();

            var books = JsonConvert.DeserializeObject<IEnumerable<Book>>(booksContent);
            var billItems = JsonConvert.DeserializeObject<IEnumerable<BillItems>>(billItemsContent);
            var Bills = JsonConvert.DeserializeObject<IEnumerable<Bill>>(billContent);
            var Input = JsonConvert.DeserializeObject<IEnumerable<InputSlip>>(InputContent);

            var thongKeSachList = new List<ThongKeViewModel>();


            var today = DateTime.Today;

            //var thongKeData = new List<ThongKeViewModel>(); // Sử dụng List thay vì IEnumerable

            thongKeSachList.AddRange( from bi in billItems
                          join b in books on bi.BookID equals b.BookID
                          join bill in Bills on bi.BillID equals bill.BillID into gBill
                          from subBill in gBill.DefaultIfEmpty() // Left Join with Bills
                          where bi.Status == 3 && subBill != null && subBill.OrderDate != null && subBill.OrderDate.Value.Date == today
                          group new { bi, b } by new { bi.BookID, b.BookName, b.QuantityExists } into g
                          select new ThongKeViewModel
                          {
                              tensach = g.Key.BookName,
                              TongSoSachBanDuoc = g.Sum(x => x.bi.Quantity),
                              SoSachConLai = g.Key.QuantityExists,
                              TongDoanhThusach = g.Sum(x => x.bi.Price * x.bi.Quantity),
                              LoiNhuansach = g.Sum(x => (x.bi.Price - x.bi.GiaNhap) * x.bi.Quantity),
                              ChiPhiGocsach = g.Sum(x => x.bi.GiaNhap * x.bi.Quantity),
                          });

            var tongTongSoSachBanDuoctrongNgay = thongKeSachList.Sum(c => c.TongSoSachBanDuoc);
            var tongTongdoanhThutrongNgay = thongKeSachList.Sum(c => c.TongDoanhThusach);
            var tongTongLoiNhuantrongNgay = thongKeSachList.Sum(c => c.LoiNhuansach);
            var tongTongChiPhigoctrongNgay = thongKeSachList.Sum(c => c.ChiPhiGocsach);

            thongKeSachList.Add(new ThongKeViewModel
            {
                tensach = "Tổng cộng",
                TongSoSachBanDuoc = tongTongSoSachBanDuoctrongNgay,
                TongDoanhThusach = tongTongdoanhThutrongNgay,
                LoiNhuansach = tongTongLoiNhuantrongNgay,
                ChiPhiGocsach = tongTongChiPhigoctrongNgay,
            });

            return Json(new
            {
                status = true,
                message = _mess,
                data = thongKeSachList
            });
        }
        [HttpPost, Route("load-data-thongke")]
        // thống kê theo điều kiện lọc 
        public async Task<IActionResult> LoaddataThongKe(filterOption filter)
        {

            bool _status = false;
            string _message = "";
            object _data = null;
            try
            {
                var urlBill = "https://localhost:7079/api/Bill/GetAllBill";
                var urlBook = "https://localhost:7079/api/Book/get-all-book";
                var urlBillitem = "https://localhost:7079/api/BillItem/GetAllBillItem";
                var urlInput = $"https://localhost:7079/api/InputSlipController/GetAllInputSlip";

                var BillksResponse = await _httpClient.GetAsync(urlBill);
                var booksResponse = await _httpClient.GetAsync(urlBook);
                var billItemsResponse = await _httpClient.GetAsync(urlBillitem);
                var InputResponse = await _httpClient.GetAsync(urlInput);

                BillksResponse.EnsureSuccessStatusCode();
                booksResponse.EnsureSuccessStatusCode();
                billItemsResponse.EnsureSuccessStatusCode();
                InputResponse.EnsureSuccessStatusCode();

                var billContent = await BillksResponse.Content.ReadAsStringAsync();
                var booksContent = await booksResponse.Content.ReadAsStringAsync();
                var billItemsContent = await billItemsResponse.Content.ReadAsStringAsync();
                var InputContent = await InputResponse.Content.ReadAsStringAsync();

                var books = JsonConvert.DeserializeObject<IEnumerable<Book>>(booksContent);
                var billItems = JsonConvert.DeserializeObject<IEnumerable<BillItems>>(billItemsContent);
                var Bills = JsonConvert.DeserializeObject<IEnumerable<Bill>>(billContent);
                var Input = JsonConvert.DeserializeObject<IEnumerable<InputSlip>>(InputContent);
                var thongKeSachList = new List<ThongKeViewModel>();



                if (BillksResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                  var result = (from bi in billItems
                                         join b in books on bi.BookID equals b.BookID join c in Bills on bi.BillID equals c.BillID 
                                         where bi.Status == 3
                                         group new { bi, b,c } by new { bi.BookID, b.BookName, b.QuantityExists,c.OrderDate } into g
                                         select new 
                                         {
                                             tensach = g.Key.BookName,
                                             TongSoSachBanDuoc = g.Sum(x => x.bi.Quantity),
                                             SoSachConLai = g.Key.QuantityExists,
                                             TongDoanhThusach = g.Sum(x => x.bi.Price * x.bi.Quantity),
                                             LoiNhuansach = g.Sum(x => (x.bi.Price - x.bi.GiaNhap) * x.bi.Quantity),
                                             ChiPhiGocsach = g.Sum(x => x.bi.GiaNhap * x.bi.Quantity),
                                            orderDate = g.Key.OrderDate
                                         });;


                    if (filter != null)
                    {
                        if (filter.NTTU_tu != null)
                        {
                            result = result.Where(c => c.orderDate >= filter.NTTU_tu.Value).ToList();

                        }

                        if (filter.NTDEN_den != null)
                        {
                            result = result.Where(c => c.orderDate <= filter.NTDEN_den.Value).ToList();

                        }

                        if (filter._search != null)
                        {
                            result = result.Where(c => c.tensach.ToLower() == filter._search.ToLower().ToString()).ToList();
                           
                        }
                    }

                    var groupedResult = result.GroupBy(c => c.tensach)
                          .Select(group => new ThongKeViewModel
                          {
                              tensach = group.Key,
                              TongSoSachBanDuoc = group.Sum(c => c.TongSoSachBanDuoc),
                              SoSachConLai = group.Sum(c => c.SoSachConLai),
                              TongDoanhThusach = group.Sum(c => c.TongDoanhThusach),
                              LoiNhuansach = group.Sum(c => c.LoiNhuansach),
                              ChiPhiGocsach = group.Sum(c => c.ChiPhiGocsach)
                          }).ToList();

                    // Tính toán tổng cộng
                    var tongSoSachBanDuocTongCong = groupedResult.Sum(c => c.TongSoSachBanDuoc);
                    var tongDoanhThuTongCong = groupedResult.Sum(c => c.TongDoanhThusach);
                    var loiNhuanTongCong = groupedResult.Sum(c => c.LoiNhuansach);
                    var chiPhiGocTongCong = groupedResult.Sum(c => c.ChiPhiGocsach);

                    groupedResult.Add(new ThongKeViewModel
                    {
                        tensach = "Tổng cộng",
                        TongSoSachBanDuoc = tongSoSachBanDuocTongCong,
                        TongDoanhThusach = tongDoanhThuTongCong,
                        LoiNhuansach = loiNhuanTongCong,
                        ChiPhiGocsach = chiPhiGocTongCong
                    });

                    _status = true;
                    _message = "lỌC DỮ LIỆU THÀNH CÔNG";
                    _data = groupedResult;
                }
                else if (BillksResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    _status = false;
                    _message = "Lỗi";

                }
                else
                {
                    _status = false;
                    _message = BillksResponse.ReasonPhrase + "";
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


        public IActionResult Chart()
        {
           
            return View();
        }
    }
}

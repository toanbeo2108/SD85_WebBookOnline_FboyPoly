using ClosedXML.Excel;
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
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        // Lấy toàn bộ thống kê
        [HttpGet, Route("GetThongKe")]
        public async Task<IActionResult> GetThongKe()
        {


            var urlBook = $"https://localhost:7079/api/Book/get-all-book";
            var urlBillitem = $"https://localhost:7079/api/BillItem/GetAllBillItem";
            var booksResponse = await _httpClient.GetAsync(urlBook);
            var billItemsResponse = await _httpClient.GetAsync(urlBillitem);

            booksResponse.EnsureSuccessStatusCode();
            billItemsResponse.EnsureSuccessStatusCode();

            var booksContent = await booksResponse.Content.ReadAsStringAsync();
            var billItemsContent = await billItemsResponse.Content.ReadAsStringAsync();

            var books = JsonConvert.DeserializeObject<IEnumerable<Book>>(booksContent);
            var billItems = JsonConvert.DeserializeObject<IEnumerable<BillItems>>(billItemsContent);

            var listThongKe = new List<ThongKeViewModel>();

            // Thống kê cho từng sách
            foreach (var book in books)
            {
                var thongKeViewModel = new ThongKeViewModel
                {
                    tensach = book.BookName,
                    TongSoSachBanDuoc = book.QuantitySold,
                    TongDoanhThusach = book.QuantitySold * book.Price,
                    ChiPhiGocsach = book.EntryPrice * book.QuantitySold,
                    LoiNhuansach = (book.QuantitySold * book.Price) - (book.EntryPrice * book.QuantitySold),
                    SoSachConLai = book.QuantityExists,


                };

                listThongKe.Add(thongKeViewModel);
            }
            var tongSoSachBanDuocTongCong = listThongKe.Sum(t => t.TongSoSachBanDuoc);
            var tongDoanhThuTongCong = listThongKe.Sum(t => t.TongDoanhThusach);
            var loiNhuanTongCong = listThongKe.Sum(t => t.LoiNhuansach);
            var chiPhiGocTongCong = listThongKe.Sum(t => t.ChiPhiGocsach);


            // Thêm vào danh sách tổng cộng
            listThongKe.Add(new ThongKeViewModel
            {
                tensach = "Tổng cộng",
                TongSoSachBanDuoc = tongSoSachBanDuocTongCong,
                TongDoanhThusach = tongDoanhThuTongCong,
                LoiNhuansach = loiNhuanTongCong,
                ChiPhiGocsach = chiPhiGocTongCong,



            });

            return Json(new
            {
                status = true,
                message = _mess,
                data = listThongKe
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

            var BillksResponse = await _httpClient.GetAsync(urlBill);
            var booksResponse = await _httpClient.GetAsync(urlBook);
            var billItemsResponse = await _httpClient.GetAsync(urlBillitem);

            BillksResponse.EnsureSuccessStatusCode();
            booksResponse.EnsureSuccessStatusCode();
            billItemsResponse.EnsureSuccessStatusCode();

            var billContent = await BillksResponse.Content.ReadAsStringAsync();
            var booksContent = await booksResponse.Content.ReadAsStringAsync();
            var billItemsContent = await billItemsResponse.Content.ReadAsStringAsync();

            var books = JsonConvert.DeserializeObject<IEnumerable<Book>>(booksContent);
            var billItems = JsonConvert.DeserializeObject<IEnumerable<BillItems>>(billItemsContent);
            var Bills = JsonConvert.DeserializeObject<IEnumerable<Bill>>(billContent);

            var thongKeSachList = new List<ThongKeViewModel>();
            var thongKeSachHashSet = new HashSet<Guid>();

            foreach (var billItem in billItems)
            {
                var billInfo = Bills.FirstOrDefault(bill => bill.BillID == billItem.BillID);
                var bookInfo = books.FirstOrDefault(book => book.BookID == billItem.BookID);

                // Kiểm tra xem ngày đặt hàng có phải là ngày hôm nay không
                if (billInfo != null && billInfo.OrderDate.HasValue && billInfo.OrderDate.Value.Date == ngayHomNay)
                {
                    var thongKeSachId = billItem.BookID.Value;

                    if (!thongKeSachHashSet.Contains(thongKeSachId))
                    {
                        var thongKeSach = new ThongKeViewModel
                        {
                            Id = thongKeSachId,
                            tensach = bookInfo.BookName,
                            TongSoSachBanDuoc = billItem.Quantity,
                            TongDoanhThusach = billItem.ToTal,
                            LoiNhuansach = billItem.ToTal - (billItem.Quantity * bookInfo.EntryPrice),
                            ChiPhiGocsach = billItem.Quantity * bookInfo.EntryPrice,
                            SoSachConLai = bookInfo.QuantityExists,
                        };

                        thongKeSachList.Add(thongKeSach);
                        thongKeSachHashSet.Add(thongKeSachId);
                    }
                    else
                    {
                        // Nếu đã có, tìm và cập nhật thông tin cho sách , không thôi kệ
                        var existingThongKeSach = thongKeSachList.First(tks => tks.Id == thongKeSachId);

                        existingThongKeSach.TongSoSachBanDuoc += billItem.Quantity;
                        existingThongKeSach.TongDoanhThusach += billItem.ToTal;
                        existingThongKeSach.LoiNhuansach += billItem.ToTal - (billItem.Quantity * bookInfo.EntryPrice);
                        existingThongKeSach.ChiPhiGocsach += billItem.Quantity * bookInfo.EntryPrice;
                        existingThongKeSach.SoSachConLai = bookInfo.QuantityExists;
                    }
                }
            }

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

                var BillksResponse = await _httpClient.GetAsync(urlBill);
                var booksResponse = await _httpClient.GetAsync(urlBook);
                var billItemsResponse = await _httpClient.GetAsync(urlBillitem);

                BillksResponse.EnsureSuccessStatusCode();
                booksResponse.EnsureSuccessStatusCode();
                billItemsResponse.EnsureSuccessStatusCode();

                var billContent = await BillksResponse.Content.ReadAsStringAsync();
                var booksContent = await booksResponse.Content.ReadAsStringAsync();
                var billItemsContent = await billItemsResponse.Content.ReadAsStringAsync();

                var books = JsonConvert.DeserializeObject<IEnumerable<Book>>(booksContent);
                var billItems = JsonConvert.DeserializeObject<IEnumerable<BillItems>>(billItemsContent);
                var Bills = JsonConvert.DeserializeObject<IEnumerable<Bill>>(billContent);

                var thongKeSachList = new List<ThongKeViewModel>();



                if (BillksResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = from b in books
                                 join bi in billItems on b.BookID equals bi.BookID
                                 join bill in Bills on bi.BillID equals bill.BillID
                                 select new
                                 {
                                     BookName = b.BookName,
                                     Quantity = bi.Quantity,
                                     Total = bi.ToTal,
                                     EntryPrice = b.EntryPrice,
                                     QuantityExists = b.QuantityExists,
                                     OrderDate = bill.OrderDate
                                 };


                    if (filter != null)
                    {
                        if (filter.NTTU_tu != null)
                        {
                            result = result.Where(c => c.OrderDate >= filter.NTTU_tu.Value).ToList();

                        }

                        if (filter.NTDEN_den != null)
                        {
                            result = result.Where(c => c.OrderDate <= filter.NTDEN_den.Value).ToList();

                        }

                        if (filter._search != null)
                        {
                            result = result.Where(c => c.BookName.ToLower() == filter._search.ToLower().ToString()).ToList();
                        }
                    }

                    var groupedResult = result.GroupBy(c => c.BookName)
                          .Select(group => new ThongKeViewModel
                          {
                              tensach = group.Key,
                              TongSoSachBanDuoc = group.Sum(c => c.Quantity),
                              SoSachConLai = group.Sum(c => c.QuantityExists),
                              TongDoanhThusach = group.Sum(c => c.Total),
                              LoiNhuansach = group.Sum(c => c.Total - (c.Quantity * c.EntryPrice)),
                              ChiPhiGocsach = group.Sum(c => c.Quantity * c.EntryPrice)
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
    }
}

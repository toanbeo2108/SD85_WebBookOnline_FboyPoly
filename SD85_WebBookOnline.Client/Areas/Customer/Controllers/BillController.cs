using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

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

            var UrlVoucher = $"https://localhost:7079/api/Voucher/GetAllVoucher";
            var responeVoucher = await _httpClient.GetAsync(UrlVoucher);
            string apiDataVoucher = await responeVoucher.Content.ReadAsStringAsync();
            var lstVoucher = JsonConvert.DeserializeObject<List<Voucher>>(apiDataVoucher);
            var voucher = lstVoucher.FirstOrDefault(p => p.VoucherID == Bill.VoucherID);

            ViewBag.Voucher = voucher;
            ViewBag.Bill = Bill;
            ViewBag.User = User;
            return View(ListBillItems);
        }

        public async Task<IActionResult> HuyHoaDon(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlBill = $"https://localhost:7079/api/Bill/GetBillByBillId/" + id;
            var responeBill = await _httpClient.GetAsync(urlBill);
            string apiBill = await responeBill.Content.ReadAsStringAsync();
            var Bill = JsonConvert.DeserializeObject<Bill>(apiBill);

            // Lấy tất cả những BillItem từ Bill trên :
            var urlBillItems = $"https://localhost:7079/api/BillItem/GetAllBillItemByBillID/" + Bill.BillID;
            var responeBillItems = await _httpClient.GetAsync(urlBillItems);
            string apiBillItems = await responeBillItems.Content.ReadAsStringAsync();
            var BillItems = JsonConvert.DeserializeObject<List<BillItems>>(apiBillItems);
            foreach (var item in BillItems)
            {
                // Cập nhật lại số lượng cho sản phẩm:
                if (item.ComboID == null) // billitem k có comboid tức là billItem của book
                {
                    // cập nhật lại sl bán được và sl tồn của sách
                    var UrlUpdateBook = $"https://localhost:7079/api/Book/PlusBook/?id={item.BookID}&quantity={item.Quantity}";
                    var contentUpdateBook = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                    var responseUpdateBook = await _httpClient.PostAsync(UrlUpdateBook, contentUpdateBook);
                    if (!responseUpdateBook.IsSuccessStatusCode)
                    {
                        return BadRequest("Lỗi cập nhật lại số lượng tồn của sản phẩm");
                    }

                    // Xóa BillItem
                    var UrlDeleteBillItem = $"https://localhost:7079/api/BillItem/DeleteBillItem/{item.BillItemID}";
                    var responseDeleteBillItem = await _httpClient.DeleteAsync(UrlDeleteBillItem);
                    var ApiDeleteBillItem = await responseDeleteBillItem.Content.ReadAsStringAsync();
                    if (!responseDeleteBillItem.IsSuccessStatusCode)
                    {
                        return BadRequest("Lỗi xóa billItems");
                    }
                }
                else
                {
                    // cập nhật lại sl bán được và sl tồn của combo
                    var UrlUpdateBook = $"https://localhost:7079/api/Combo/CancelBill/?id={item.ComboID}&quantityBuy={item.Quantity}";
                    var contentUpdateBook = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                    var responseUpdateBook = await _httpClient.PostAsync(UrlUpdateBook, contentUpdateBook);
                    if (!responseUpdateBook.IsSuccessStatusCode)
                    {
                        return BadRequest("Lỗi cập nhật lại số lượng tồn của sản phẩm");
                    }

                    // Xóa BillItem
                    var UrlDeleteBillItem = $"https://localhost:7079/api/BillItem/DeleteBillItem/{item.BillItemID}";
                    var responseDeleteBillItem = await _httpClient.DeleteAsync(UrlDeleteBillItem);
                    var ApiDeleteBillItem = await responseDeleteBillItem.Content.ReadAsStringAsync();
                    if (!responseDeleteBillItem.IsSuccessStatusCode)
                    {
                        return BadRequest("Lỗi xóa billItems");
                    }
                }
            }

            // Sau khi xóa BillItem thì Xóa bill
            var UrlDeleteBill = $"https://localhost:7079/api/Bill/DeleteBill/{id}";
            var responseDeleteBill = await _httpClient.DeleteAsync(UrlDeleteBill);
            var ApiDeleteBill = await responseDeleteBill.Content.ReadAsStringAsync();
            if (!responseDeleteBill.IsSuccessStatusCode)
            {
                return BadRequest("Lỗi xóa Bill");
            }
            else 
            {
                return RedirectToAction("GetAllBill");
            }    
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Client.Controllers;
using SD85_WebBookOnline.Client.Services;
using SD85_WebBookOnline.Share.Models;
using SD85_WebBookOnline.Share.ViewModels;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Helpers;
using System.Xml.Linq;

namespace SD85_WebBookOnline.Client.Areas.Customer.Controllers
{
    public class CartController : Controller
    {
        
        private readonly HttpClient _httpClient;
        public List<CartItems> CartItemss { get; set; } = new List<CartItems>();
        public CustomsApi<Book> _BookApi;
        public CartController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> deTail(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var url = $"https://localhost:7079/api/user/GetAllUser";
            var response = await _httpClient.GetAsync(url);
            string apiDataUser = await response.Content.ReadAsStringAsync();
            var ListUser = JsonConvert.DeserializeObject<List<User>>(apiDataUser);
            ViewBag.ListUser = ListUser;
            
            var urlRating = $"https://localhost:7079/api/Rating/GetAllRating";
            var httpClient = new HttpClient();
            var responRating = await _httpClient.GetAsync(urlRating);
            string apiDataRating = await responRating.Content.ReadAsStringAsync();
            var lstRating = JsonConvert.DeserializeObject<List<Rating>>(apiDataRating);
            var lstRating_book = lstRating.Where(x => x.IdBook == id).ToList();
            ViewBag.lstRating_book = lstRating_book;
            int countStar = lstRating_book.Count(x => x.IdBook == id);
            ViewBag.countStar = countStar;
            decimal sumStar = (decimal)lstRating_book.Sum(x => x.Stars);
            ViewBag.sumStar = sumStar;
            decimal averageStars = sumStar / countStar;
            ViewBag.AverageStars = averageStars;
            var lstSelectRating = lstRating_book.OrderByDescending(x => x.RatingDate).Take(4).ToList();
            ViewBag.lstSelectRating = lstSelectRating;


            var urlBook = $"https://localhost:7079/api/Book/get-all-book";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.BookID == id);
            if (Book == null)
            {
                return BadRequest();
            }
            else
            {
                ViewBag.BookDetail = Book;

                

                var urlImage = $"https://localhost:7079/api/Image/getAll_Image";
                var responImage = await _httpClient.GetAsync(urlImage);
                string apiDataImage = await responImage.Content.ReadAsStringAsync();
                var lstImage = JsonConvert.DeserializeObject<List<Images>>(apiDataImage);
                var lstImageBookDetail = lstImage.Where(x => x.BookID == Book.BookID).ToList();
                if (lstImageBookDetail != null)
                {
                    ViewBag.lstImageBookDetail = lstImageBookDetail;
                }
                return View(Book);
            }
        }
        public async Task<IActionResult> AddToCart(Guid id, int quantity)
        {
            // lấy list book
            var urlBook = "https://localhost:7079/api/Book/GetBookByID/"+id;
            var httpClient = new HttpClient();
            var responseBook = await httpClient.GetAsync(urlBook);
            if (!responseBook.IsSuccessStatusCode)
            {
                return BadRequest("Lỗi khi tải danh sách sách.");
            }
            string apiDataBook = await responseBook.Content.ReadAsStringAsync();
            var book = JsonConvert.DeserializeObject<Book>(apiDataBook);

            // lấy list sách
            var urlCombo = $"https://localhost:7079/api/Combo/GetAllCombo";
            var responCombo = await _httpClient.GetAsync(urlCombo);
            string apiDataCombo = await responCombo.Content.ReadAsStringAsync();
            var lstCombo = JsonConvert.DeserializeObject<List<Combo>>(apiDataCombo);
            var combo = lstCombo.FirstOrDefault(x => x.ComboID == id);

            // Lấy danh sách giỏ hàng của người dùng chưa thanh toán
            string UserId = Request.Cookies["UserID"];
            var urlCart = $"https://localhost:7079/api/Cart/GetCartByIdUser/{UserId}?status=1";
            var responCart = await _httpClient.GetAsync(urlCart);
            string apiDataCart = await responCart.Content.ReadAsStringAsync();
            var ListCart = JsonConvert.DeserializeObject<List<Cart>>(apiDataCart);
            
            if (ListCart.Count() == 0)
            {
                Cart cart = new Cart();
                cart.CartId = Guid.NewGuid();
                cart.VoucherID = null;
                cart.UserID = UserId;
                cart.PriceBeforeVoucher = 0;
                cart.Total = 0;
                cart.Status = 1;
                var urlCreateCart = $"https://localhost:7079/api/Cart/CreateCart?CartId={cart.CartId}&voucherID={cart.VoucherID}&UserId={cart.UserID}&priceBeforeVoucher={cart.PriceBeforeVoucher}&total={cart.Total}";
                var contentCreateCart = new StringContent(JsonConvert.SerializeObject(cart), Encoding.UTF8, "application/json");
                var responCreateCart = await _httpClient.PostAsync(urlCreateCart, contentCreateCart);
                if (!responCreateCart.IsSuccessStatusCode)
                {
                    return BadRequest("Lỗi tạo 1 giỏ hàng mới cho khách hàng");
                }

                // Tạo xong giỏ hàng thì tạo cartitem mới để thêm vào giỏ hàng mới
                CartItems cartItems = new CartItems();
                cartItems.CartItemID = Guid.NewGuid();
                cartItems.CartID = null;
                if (book != null)
                {
                    cartItems.BookID = book.BookID;
                    cartItems.ComboID = null;
                    cartItems.ItemName = book.BookName;
                    cartItems.Image = book.MainPhoto;
                    cartItems.Price = book.Price;
                    cartItems.Quantity = quantity;
                    cartItems.ToTal = cartItems.Price * cartItems.Quantity;
                    cartItems.Status = 1;
                }
                if (combo != null)
                {
                    cartItems.BookID = null;
                    cartItems.ComboID = combo.ComboID;
                    cartItems.ItemName = combo.ComboName;
                    cartItems.Image = combo.Image;
                    cartItems.Price = combo.Price;
                    cartItems.Quantity = quantity;
                    cartItems.ToTal = cartItems.Price * cartItems.Quantity;
                    cartItems.Status = 1;
                }
                // Dùng API thêm vào database
                var urlCreateCartItems = $"https://localhost:7079/api/CartItem/Add-CartItem?CartID={cart.CartId}&BookID={cartItems.BookID}&image={cartItems.Image}&ItemName={cartItems.ItemName}&Price={cartItems.Price}&Quantity={cartItems.Quantity}&ToTal={cartItems.ToTal}&Status={cartItems.Status}";
                var contentCreateCartItems = new StringContent(JsonConvert.SerializeObject(cartItems), Encoding.UTF8, "application/json");
                var responCreateCartItems = await _httpClient.PostAsync(urlCreateCartItems, contentCreateCartItems);
                if (!responCreateCartItems.IsSuccessStatusCode)
                {
                    return BadRequest("Lỗi thêm vào chi tiết giỏ hàng ( CartItems )");
                }
            }

            // Lấy được danh sách CartItems dựa theo những Cart chưa thanh toán kia :
            foreach (var Cart in ListCart)
            {
                var urlCartItems = $"https://localhost:7079/api/CartItem/GetCartItemByCartID/{Cart.CartId}";
                var responCartItems = await _httpClient.GetAsync(urlCartItems);
                string apiDataCartItems = await responCartItems.Content.ReadAsStringAsync();
                var ListCartItems = JsonConvert.DeserializeObject<List<CartItems>>(apiDataCartItems);

                // Kiểm tra xem sản phẩm tồn tại trong giỏ hàng chưa
                CartItems existingItem = ListCartItems.FirstOrDefault(x => x.BookID == book.BookID);
                if (existingItem != null)
                {
                    // Nếu sách đã có, tăng số lượng lên 
                    existingItem.Quantity += quantity;
                    existingItem.ToTal = existingItem.Price * existingItem.Quantity;

                    // Cập nhật lại trong database
                    var urlUpdateCartItems = $"https://localhost:7079/api/CartItem/Update-CartItem/{existingItem.CartItemID}";
                    var contentUpdateCartItems = new StringContent(JsonConvert.SerializeObject(existingItem), Encoding.UTF8, "application/json");
                    var responeUpdateCartItems = await _httpClient.PutAsync(urlUpdateCartItems, contentUpdateCartItems);
                    if (!responeUpdateCartItems.IsSuccessStatusCode)
                    {
                        return BadRequest("Lỗi ko thể cập nhật lại số lượng sản phẩm");
                    }
                }
                else
                {
                    CartItems cartItems = new CartItems();
                    cartItems.CartItemID = Guid.NewGuid();
                    cartItems.CartID = null;
                    if (book != null)
                    {
                        cartItems.BookID = book.BookID;
                        cartItems.ComboID = null;
                        cartItems.ItemName = book.BookName;
                        cartItems.Image = book.MainPhoto;
                        cartItems.Price = book.Price;
                        cartItems.Quantity = quantity;
                        cartItems.ToTal = cartItems.Price * cartItems.Quantity;
                        cartItems.Status = 1;
                    }
                    if (combo != null)
                    {
                        cartItems.BookID = null;
                        cartItems.ComboID = combo.ComboID;
                        cartItems.ItemName = combo.ComboName;
                        cartItems.Image = combo.Image;
                        cartItems.Price = combo.Price;
                        cartItems.Quantity = quantity;
                        cartItems.ToTal = cartItems.Price * cartItems.Quantity;
                        cartItems.Status = 1;
                    }
                    // Dùng API thêm vào database
                    var urlCreateCartItems = $"https://localhost:7079/api/CartItem/Add-CartItem?CartID={Cart.CartId}&BookID={cartItems.BookID}&image={cartItems.Image}&ItemName={cartItems.ItemName}&Price={cartItems.Price}&Quantity={cartItems.Quantity}&ToTal={cartItems.ToTal}&Status={cartItems.Status}";
                    var contentCreateCartItems = new StringContent(JsonConvert.SerializeObject(cartItems), Encoding.UTF8, "application/json");
                    var responCreateCartItems = await _httpClient.PostAsync(urlCreateCartItems, contentCreateCartItems);
                    if (!responCreateCartItems.IsSuccessStatusCode)
                    {
                        return BadRequest("Lỗi thêm vào chi tiết giỏ hàng ( CartItems )");
                    }
                }

            }
            return RedirectToAction("MyCart", "Cart", new { area = "Customer" });

        }
        [HttpGet]
        public async Task<IActionResult> MyCart()
        {
            string UserId = Request.Cookies["UserID"];
            if (UserId == null)
            {
                return BadRequest("Lỗi không tìm đc UserID");
            }
            else if(UserId != null)
            {
                // Lấy tất cả giỏ hàng của khách hàng có trạng thái == 1 ( chưa thanh toán )
                var urlCart = $"https://localhost:7079/api/Cart/GetCartByIdUser/{UserId}?status=1";
                var responCart = await _httpClient.GetAsync(urlCart);
                string apiDataCart = await responCart.Content.ReadAsStringAsync();
                var ListCart = JsonConvert.DeserializeObject<List<Cart>>(apiDataCart);

                // Dùng vòng lặp để lấy được các CartItem dựa vào CartID
                foreach (var Cart in ListCart)
                {
                    var urlCartItems = $"https://localhost:7079/api/CartItem/GetCartItemByCartID/{Cart.CartId}";
                    var responCartItems = await _httpClient.GetAsync(urlCartItems);
                    string apiDataCartItems = await responCartItems.Content.ReadAsStringAsync();
                    List<CartItems> ListCartItems = JsonConvert.DeserializeObject<List<CartItems>>(apiDataCartItems);

                    ViewBag.myCartItems = ListCartItems;
                    decimal subtotal = 0;
                    foreach (var item in ListCartItems)
                    {
                        subtotal += item.ToTal;
                    }
                    if (subtotal == 0)
                    {
                        ViewBag.Subtotal = 0;
                    }
                    else
                    {
                        ViewBag.Subtotal = subtotal;
                    }

                }
            }
            return View();
        }

        public async Task<IActionResult> DeleteToCart(Guid id)
        {
            // Lấy CartItem bằng ID :
            var urlCartItems = $"https://localhost:7079/api/CartItem/GetAll-CartItem";
            var responCartItems = await _httpClient.GetAsync(urlCartItems);
            string apiDataCartItems = await responCartItems.Content.ReadAsStringAsync();
            List<CartItems> ListCartItems = JsonConvert.DeserializeObject<List<CartItems>>(apiDataCartItems);
            CartItems CartItemDelete = ListCartItems.FirstOrDefault(p => p.BookID == id);

            // Gọi API Xóa CartItem :
            var url = $"https://localhost:7079/api/CartItem/Delete-CartItem/{CartItemDelete.CartItemID}";
            var responCreateCartItems = await _httpClient.DeleteAsync(url);

            return RedirectToAction("MyCart", "Cart", new { area = "Customer" });
        }

        [HttpGet] // Mở Form
        public async Task<IActionResult> Checkout()
        {
            string UserId = Request.Cookies["UserID"];
            if (UserId == null)
            {
                return BadRequest("Lỗi không tìm đc UserID");
            }
            else if (UserId != null)
            {
                // Lấy tất cả giỏ hàng của khách hàng có trạng thái == 1 ( chưa thanh toán )
                var urlCart = $"https://localhost:7079/api/Cart/GetCartByIdUser/{UserId}?status=1";
                var responCart = await _httpClient.GetAsync(urlCart);
                string apiDataCart = await responCart.Content.ReadAsStringAsync();
                var ListCart = JsonConvert.DeserializeObject<List<Cart>>(apiDataCart);

                // Dùng vòng lặp để lấy được các CartItem dựa vào CartID
                foreach (var Cart in ListCart)
                {
                    var urlCartItems = $"https://localhost:7079/api/CartItem/GetCartItemByCartID/{Cart.CartId}";
                    var responCartItems = await _httpClient.GetAsync(urlCartItems);
                    string apiDataCartItems = await responCartItems.Content.ReadAsStringAsync();
                    List<CartItems> ListCartItems = JsonConvert.DeserializeObject<List<CartItems>>(apiDataCartItems);

                    ViewBag.myCartItems = ListCartItems;
                    decimal subtotal = 0;
                    foreach (var item in ListCartItems)
                    {
                        subtotal += item.ToTal;
                    }
                    if (subtotal == 0)
                    {
                        ViewBag.Subtotal = 0;
                    }
                    else
                    {
                        ViewBag.Subtotal = subtotal;
                    }

                }
            }
            return View();
        }
        public async Task<IActionResult> Test()
        {
            return View();
        }

        [HttpPost] // Gửi
        public async Task<IActionResult> Checkout_saveBill(SaveBillViewModel ModelBill, decimal Total)
        {
            // Authorize
            var UserId = Request.Cookies["UserID"];

            var UrlLocation = "https://raw.githubusercontent.com/kenzouno1/DiaGioiHanhChinhVN/master/data.json";
            var responseLocation = await _httpClient.GetAsync(UrlLocation);
            var jsonLocation = await responseLocation.Content.ReadAsStringAsync();
            List<SD85_WebBookOnline.Share.ViewModels.Location.City> dataLocation = JsonConvert.DeserializeObject<List<SD85_WebBookOnline.Share.ViewModels.Location.City>>(jsonLocation);
            var city = dataLocation.FirstOrDefault(c => c.Id == ModelBill.City);
            var district = city?.Districts.FirstOrDefault(d => d.Id == ModelBill.District);
            var ward = district?.Wards.FirstOrDefault(w => w.Id == ModelBill.Ward);

            // đầu tiên cần check xem số lượng trong giỏ hàng có đủ trong db không

            // Lấy tất cả CartItems của User
            var urlCart = $"https://localhost:7079/api/Cart/GetCartByIdUser/{UserId}?status=1";
            var responCart = await _httpClient.GetAsync(urlCart);
            string apiDataCart = await responCart.Content.ReadAsStringAsync();
            var ListCart = JsonConvert.DeserializeObject<List<Cart>>(apiDataCart);
            foreach (var Cart in ListCart)
            {
                var urlCartItems = $"https://localhost:7079/api/CartItem/GetCartItemByCartID/{Cart.CartId}";
                var responCartItems = await _httpClient.GetAsync(urlCartItems);
                string apiDataCartItems = await responCartItems.Content.ReadAsStringAsync();
                List<CartItems> ListCartItems = JsonConvert.DeserializeObject<List<CartItems>>(apiDataCartItems);

                foreach (var item in ListCartItems)
                {
                    var UrlCheck = $"https://localhost:7079/api/CartItem/CheckQuantity?IdCartItems={item.CartItemID}&QuantityCartItem={item.Quantity}";
                    var contentCheck = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                    var responseCheck = await _httpClient.PostAsync(UrlCheck, contentCheck);
                    if (responseCheck.IsSuccessStatusCode)
                    {
                        string result = await responseCheck.Content.ReadAsStringAsync();
                        var KqCheck = JsonConvert.DeserializeObject<bool>(result);
                        if (KqCheck != true)
                        {
                            return BadRequest("Số lượng sản phẩm không đáp ứng đủ, Hãy thử xóa sản phẩm '" + item.ItemName + "' ra khỏi giỏ hàng");
                        }
                    }
                    else
                    {
                        return BadRequest("Lỗi kiểm tra số lượng sản phẩm trong giỏ");
                    }
                }
            }
            // Tạo 1 Bill :
            var newBillId = Guid.NewGuid();
            Bill newBill = new Bill();
            newBill.BillID = newBillId;
            newBill.UserID = UserId;
            newBill.Email = ModelBill.Email;
            newBill.ReceiverName = ModelBill.firstName + " " + ModelBill.lastName;
            newBill.UserPhone = ModelBill.UserPhone;
            if (ModelBill.Street != null)
            {
                newBill.AddressUser = city.Name + "," + district.Name + "," + ward.Name + "," + ModelBill.Street;
            }
            else
            {
                newBill.AddressUser = city.Name + "," + district.Name + "," + ward.Name;
            }
            newBill.Total = Total;
            newBill.Shipmoney = 10;
            newBill.PaymentMethod = ModelBill.PaymentMethod;
            newBill.Status = 1;

            // Lưu Bill vào cơ sở dữ liệu :
            var urlBill = $"https://localhost:7079/api/Bill/CreateBillWithManualBillId?priceBeforeVoucher={newBill.Total}&ReceiverName={newBill.ReceiverName}&Email={newBill.Email}&BillID={newBill.BillID}&UserID={newBill.UserID}&shipmoney={newBill.Shipmoney}&userPhone={newBill.UserPhone}&addressUser={newBill.AddressUser}&orderDate={DateTime.Today}&deliveryDate={DateTime.Today.AddDays(5)}&total={newBill.Total}&paymentMethod={newBill.PaymentMethod}&status={newBill.Status}";
            var content = new StringContent(JsonConvert.SerializeObject(newBill), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(urlBill, content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Thêm bill mới thất Bại";
                return View();
            }
            else
            {
                // Nếu tạo thành công bill thì sẽ tạo các BillIItems
                foreach (var Cart in ListCart)
                {
                    var urlCartItems = $"https://localhost:7079/api/CartItem/GetCartItemByCartID/{Cart.CartId}";
                    var responCartItems = await _httpClient.GetAsync(urlCartItems);
                    string apiDataCartItems = await responCartItems.Content.ReadAsStringAsync();
                    List<CartItems> ListCartItems = JsonConvert.DeserializeObject<List<CartItems>>(apiDataCartItems);

                    foreach (var item in ListCartItems)
                    {
                        // Tạo các BillItems từ các CartItem đã lưu :
                        BillItems billItems = new BillItems();
                        billItems.BillItemID = Guid.NewGuid();
                        billItems.BillID = newBillId;
                        billItems.BookID = item.BookID;
                        billItems.ItemName = item.ItemName;
                        billItems.Price = item.Price;
                        billItems.Quantity = item.Quantity;
                        billItems.ToTal = billItems.Quantity * billItems.Price;
                        billItems.Status = 1;

                        // Sau khi tạo xong thì lưu nó vô database
                        var urlSaveBillItems = $"https://localhost:7079/api/BillItem/CreateBillItem?bookid={billItems.BookID}&billid={billItems.BillID}&itemname={billItems.ItemName}&price={billItems.Price}&quantity={billItems.Quantity}&total={billItems.ToTal}";
                        var contentBillItem = new StringContent(JsonConvert.SerializeObject(billItems), Encoding.UTF8, "application/json");
                        var responseCBIT = await _httpClient.PostAsync(urlSaveBillItems, contentBillItem);
                        if (!responseCBIT.IsSuccessStatusCode)
                        {
                            return BadRequest("Thêm không thành công");
                        }
                        // Cập nhật lại số lượng sản phẩm trong database
                        var urlUpdateQuantity = $"https://localhost:7079/api/Book/BuyBook?id={item.BookID}&quantityBuy={item.Quantity}";
                        var contentUpdateQuantity = new StringContent(JsonConvert.SerializeObject(billItems), Encoding.UTF8, "application/json");
                        var responseUpdateQuantity = await _httpClient.PutAsync(urlUpdateQuantity, contentBillItem);
                        if (!responseUpdateQuantity.IsSuccessStatusCode)
                        {
                            return BadRequest("Lỗi cập nhật lại số lượng sản phẩm");
                        }
                        // Xóa sản phẩm ra khỏi giỏ hàng 
                        var urlDelete = $"https://localhost:7079/api/CartItem/Delete-CartItem/{item.CartItemID}";
                        var responDelete = await _httpClient.DeleteAsync(urlDelete);
                        if (!responDelete.IsSuccessStatusCode)
                        {
                            return BadRequest("Lỗi xóa sản phẩm khỏi giỏ hàng");
                        }
                    }
                }
                return RedirectToAction("GetAllBill", "Bill");
            }
            
        }
        [HttpGet]

        public async Task<IActionResult> Book_AllRating(Guid id)
        {

            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            var url = $"https://localhost:7079/api/user/GetAllUser";
            var response = await _httpClient.GetAsync(url);
            string apiDataUser = await response.Content.ReadAsStringAsync();
            var ListUser = JsonConvert.DeserializeObject<List<User>>(apiDataUser);
            ViewBag.listUser = ListUser;


            var urlRating = $"https://localhost:7079/api/Rating/GetAllRating";
            var httpClient = new HttpClient();
            var responRating = await _httpClient.GetAsync(urlRating);
            string apiDataRating = await responRating.Content.ReadAsStringAsync();
            var lstRating = JsonConvert.DeserializeObject<List<Rating>>(apiDataRating);
            var Rating = lstRating.FirstOrDefault(x => x.IdBook == id);
            if (Rating == null)
            {
                return BadRequest("Sản phẩm này ko có bình luận nào");
            }
            else
            {
                ViewBag.lstRating = lstRating;
                return View(lstRating);
            }

        }
        [HttpGet]
        public async Task<IActionResult> CreateRating(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var urlBook = $"https://localhost:7079/api/Book/get-all-book";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            var Book = lstBook.FirstOrDefault(x => x.BookID == id);
            ViewBag.Book = Book;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRating(Rating bk,Guid id)
        {
            var UserId = Request.Cookies["UserID"];
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            bk.ID = Guid.NewGuid();
            bk.IdBook = id;
            bk.IdNguoiDung = UserId;
            bk.RatingDate = DateTime.Now;
            bk.Status = 0;

            var urlBook = $"https://localhost:7079/api/Rating/CreateRating?idbook={id}&iduser={UserId}&comment={bk.Comment}&stars={bk.Stars}";
            var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(bk), Encoding.UTF8, "application/json");
            var respon = await httpClient.PostAsync(urlBook, content);
            if (respon.IsSuccessStatusCode)
            {
                return RedirectToAction("deTail", "Cart", new { Area = "Customer", id = id });
            }
            TempData["erro message"] = "thêm thất bại";
            return View();
        }
    }
}

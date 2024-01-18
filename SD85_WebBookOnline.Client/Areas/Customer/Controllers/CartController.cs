using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Client.Controllers;
using SD85_WebBookOnline.Client.Models;
using SD85_WebBookOnline.Client.Services;
using SD85_WebBookOnline.Share.Models;
using SD85_WebBookOnline.Share.ViewModels;
using System.Globalization;
using System.Net.Http;
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

        public int QuantityPro { get;set; }

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
            double sumStar = (double)lstRating_book.Sum(x => x.Stars);
            ViewBag.sumStar = sumStar;
            //decimal averageStars = sumStar / countStar;
            //ViewBag.AverageStars = averageStars;
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
        [HttpGet]
        public async Task<IActionResult> Detail_cb(Guid id)

        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlCombo = $"https://localhost:7079/api/Combo/GetAllCombo";
            var httpClient = new HttpClient();
            var responCombo = await _httpClient.GetAsync(urlCombo);
            string apiDataCombo = await responCombo.Content.ReadAsStringAsync();
            var lstCombo = JsonConvert.DeserializeObject<List<Combo>>(apiDataCombo);
            var combo = lstCombo.FirstOrDefault(x => x.ComboID == id);
            if (combo == null)
            {
                return NotFound("Combo này đang null, kiểm tra lại dùm");
            }
            string json = Request.Cookies["lstComboItem"];
            if (json != null)
            {
                List<ComboItem> myList = JsonConvert.DeserializeObject<List<ComboItem>>(json);
                ViewBag.ListComboItem = myList;

            }
            ViewBag.combo = combo;
            return View();
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
                    // Dùng API thêm vào database
                    var urlCreateCartItems = $"https://localhost:7079/api/CartItem/Add-CartItem?CartID={cart.CartId}&BookID={cartItems.BookID}&image={cartItems.Image}&ItemName={cartItems.ItemName}&Price={cartItems.Price}&Quantity={cartItems.Quantity}&ToTal={cartItems.ToTal}&Status={cartItems.Status}";
                    var contentCreateCartItems = new StringContent(JsonConvert.SerializeObject(cartItems), Encoding.UTF8, "application/json");
                    var responCreateCartItems = await _httpClient.PostAsync(urlCreateCartItems, contentCreateCartItems);
                    if (!responCreateCartItems.IsSuccessStatusCode)
                    {
                        return BadRequest("Lỗi thêm vào chi tiết giỏ hàng ( CartItems )");
                    }

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
                    // Dùng API thêm vào database
                    var urlCreateCartItems = $"https://localhost:7079/api/CartItem/Add-CartItem?CartID={cart.CartId}&ComboID={cartItems.ComboID}&image={cartItems.Image}&ItemName={cartItems.ItemName}&Price={cartItems.Price}&Quantity={cartItems.Quantity}&ToTal={cartItems.ToTal}&Status={cartItems.Status}";
                    var contentCreateCartItems = new StringContent(JsonConvert.SerializeObject(cartItems), Encoding.UTF8, "application/json");
                    var responCreateCartItems = await _httpClient.PostAsync(urlCreateCartItems, contentCreateCartItems);
                    if (!responCreateCartItems.IsSuccessStatusCode)
                    {
                        return BadRequest("Lỗi thêm vào chi tiết giỏ hàng ( CartItems )");
                    }
                }
               
            }

            // Lấy được danh sách CartItems dựa theo những Cart chưa thanh toán kia :
            foreach (var Cart in ListCart)
            {
                var urlCartItems = $"https://localhost:7079/api/CartItem/GetCartItemByCartID/{Cart.CartId}";
                var responCartItems = await _httpClient.GetAsync(urlCartItems);
                string apiDataCartItems = await responCartItems.Content.ReadAsStringAsync();
                var ListCartItems = JsonConvert.DeserializeObject<List<CartItems>>(apiDataCartItems);
                if (combo != null)
                {
                    // Kiểm tra xem sản phẩm tồn tại trong giỏ hàng chưa
                    CartItems existingItem = ListCartItems.FirstOrDefault(x => x.ComboID == combo.ComboID);
                    // Kiểm tra xem sản phẩm tồn tại trong giỏ hàng chưa
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
                            // Dùng API thêm vào database
                            var urlCreateCartItems = $"https://localhost:7079/api/CartItem/Add-CartItem?CartID={Cart.CartId}&BookID={cartItems.BookID}&image={cartItems.Image}&ItemName={cartItems.ItemName}&Price={cartItems.Price}&Quantity={cartItems.Quantity}&ToTal={cartItems.ToTal}&Status={cartItems.Status}";
                            var contentCreateCartItems = new StringContent(JsonConvert.SerializeObject(cartItems), Encoding.UTF8, "application/json");
                            var responCreateCartItems = await _httpClient.PostAsync(urlCreateCartItems, contentCreateCartItems);
                            if (!responCreateCartItems.IsSuccessStatusCode)
                            {
                                return BadRequest("Lỗi thêm vào chi tiết giỏ hàng ( CartItems )");
                            }

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

                            // Dùng API thêm vào database
                            var urlCreateCartItems = $"https://localhost:7079/api/CartItem/Add-CartItem?CartID={Cart.CartId}&ComboID={cartItems.ComboID}&image={cartItems.Image}&ItemName={cartItems.ItemName}&Price={cartItems.Price}&Quantity={cartItems.Quantity}&ToTal={cartItems.ToTal}&Status={cartItems.Status}";
                            var contentCreateCartItems = new StringContent(JsonConvert.SerializeObject(cartItems), Encoding.UTF8, "application/json");
                            var responCreateCartItems = await _httpClient.PostAsync(urlCreateCartItems, contentCreateCartItems);
                            if (!responCreateCartItems.IsSuccessStatusCode)
                            {
                                return BadRequest("Lỗi thêm vào chi tiết giỏ hàng ( CartItems )");
                            }
                        }

                    }
                }
                else
                {
                    CartItems existingItem = ListCartItems.FirstOrDefault(x => x.BookID == book.BookID);
                    // Kiểm tra xem sản phẩm tồn tại trong giỏ hàng chưa
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
                            // Dùng API thêm vào database
                            var urlCreateCartItems = $"https://localhost:7079/api/CartItem/Add-CartItem?CartID={Cart.CartId}&BookID={cartItems.BookID}&image={cartItems.Image}&ItemName={cartItems.ItemName}&Price={cartItems.Price}&Quantity={cartItems.Quantity}&ToTal={cartItems.ToTal}&Status={cartItems.Status}";
                            var contentCreateCartItems = new StringContent(JsonConvert.SerializeObject(cartItems), Encoding.UTF8, "application/json");
                            var responCreateCartItems = await _httpClient.PostAsync(urlCreateCartItems, contentCreateCartItems);
                            if (!responCreateCartItems.IsSuccessStatusCode)
                            {
                                return BadRequest("Lỗi thêm vào chi tiết giỏ hàng ( CartItems )");
                            }

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

                            // Dùng API thêm vào database
                            var urlCreateCartItems = $"https://localhost:7079/api/CartItem/Add-CartItem?CartID={Cart.CartId}&ComboID={cartItems.ComboID}&image={cartItems.Image}&ItemName={cartItems.ItemName}&Price={cartItems.Price}&Quantity={cartItems.Quantity}&ToTal={cartItems.ToTal}&Status={cartItems.Status}";
                            var contentCreateCartItems = new StringContent(JsonConvert.SerializeObject(cartItems), Encoding.UTF8, "application/json");
                            var responCreateCartItems = await _httpClient.PostAsync(urlCreateCartItems, contentCreateCartItems);
                            if (!responCreateCartItems.IsSuccessStatusCode)
                            {
                                return BadRequest("Lỗi thêm vào chi tiết giỏ hàng ( CartItems )");
                            }
                        }

                    }
                }
            }
            return RedirectToAction("MyCart", "Cart", new { area = "Customer" });

        }

        public async Task<IActionResult> PlusCartItem(Guid id)
        {
            // lấy list book
            var urlBook = "https://localhost:7079/api/Book/GetBookByID/" + id;
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

            foreach (var Cart in ListCart)
            {
                var urlCartItems = $"https://localhost:7079/api/CartItem/GetCartItemByCartID/{Cart.CartId}";
                var responCartItems = await _httpClient.GetAsync(urlCartItems);
                string apiDataCartItems = await responCartItems.Content.ReadAsStringAsync();
                var ListCartItems = JsonConvert.DeserializeObject<List<CartItems>>(apiDataCartItems);
                if (combo != null)
                {
                    // Tăng số lượng của CartItem lên
                    CartItems existingItem = ListCartItems.FirstOrDefault(x => x.ComboID == combo.ComboID);
                    if (existingItem == null)
                    {
                        return BadRequest("Lỗi sản phẩm này chưa tồn tại trong giỏ hàng");
                    }
                    existingItem.Quantity += 1;
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
                    // Tăng số lượng của CartItem lên
                    CartItems existingItem = ListCartItems.FirstOrDefault(x => x.BookID == book.BookID);
                    if (existingItem == null)
                    {
                        return BadRequest("Lỗi sản phẩm này chưa tồn tại trong giỏ hàng");
                    }
                    existingItem.Quantity += 1;
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

            }

            return RedirectToAction("MyCart", "Cart");
        }
        public async Task<IActionResult> MinusCartItem(Guid id)
        {
            // lấy list book
            var urlBook = "https://localhost:7079/api/Book/GetBookByID/" + id;
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

            foreach (var Cart in ListCart)
            {
                var urlCartItems = $"https://localhost:7079/api/CartItem/GetCartItemByCartID/{Cart.CartId}";
                var responCartItems = await _httpClient.GetAsync(urlCartItems);
                string apiDataCartItems = await responCartItems.Content.ReadAsStringAsync();
                var ListCartItems = JsonConvert.DeserializeObject<List<CartItems>>(apiDataCartItems);
                if (combo != null)
                {
                    // Tăng số lượng của CartItem lên
                    CartItems existingItem = ListCartItems.FirstOrDefault(x => x.ComboID == combo.ComboID);
                    if (existingItem == null)
                    {
                        return BadRequest("Lỗi sản phẩm này chưa tồn tại trong giỏ hàng");
                    }
                    existingItem.Quantity -= 1;
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
                    // Tăng số lượng của CartItem lên
                    CartItems existingItem = ListCartItems.FirstOrDefault(x => x.BookID == book.BookID);
                    if (existingItem == null)
                    {
                        return BadRequest("Lỗi sản phẩm này chưa tồn tại trong giỏ hàng");
                    }
                    existingItem.Quantity -= 1;
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

            }

            return RedirectToAction("MyCart", "Cart");
        }



        [HttpGet] 
        public async Task<IActionResult> GetVoucherByCondition(decimal subtotal)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlVoucher = $"https://localhost:7079/api/Voucher/GetAllVoucher";
            var responVoucher = await _httpClient.GetAsync(urlVoucher);
            string apiDataVoucher = await responVoucher.Content.ReadAsStringAsync();
            var lstVoucher = JsonConvert.DeserializeObject<IEnumerable<Voucher>>(apiDataVoucher);
            var lstVoucherok = lstVoucher.Where(x => x.Quantity > 0 && x.EndDate > DateTime.Now && x.Status > 0).ToList();
            var lstvcFreeShip = lstVoucherok.Where(x => x.Status == 1 && x.DiscountCondition <= subtotal).ToList();
            var lstvcGiamGia = lstVoucherok.Where(x => x.Status == 2 && x.DiscountCondition <= subtotal).ToList();
            ViewBag.lstVoucherok = lstVoucherok;
            ViewBag.lstvcFreeShip = lstvcFreeShip;
            ViewBag.lstvcGiamGia = lstvcGiamGia;
            return View();

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
            CartItems CartItemDelete = ListCartItems.FirstOrDefault(p => p.BookID == id || p.ComboID == id);

            // Gọi API Xóa CartItem :
            var url = $"https://localhost:7079/api/CartItem/Delete-CartItem/{CartItemDelete.CartItemID}";
            var responCreateCartItems = await _httpClient.DeleteAsync(url);

            return RedirectToAction("MyCart", "Cart", new { area = "Customer" });
        }

        [HttpGet] // Mở Form
        public async Task<IActionResult> Checkout(string? Voucher_code)
        {
           
            if (Voucher_code != null)
            {
                Response.Cookies.Append("Voucher_id", Voucher_code);
            }
            var UrlVoucher = $"https://localhost:7079/api/Voucher/GetVoucherByVoucherCode?VoucherCode={Voucher_code}";
            var responeVoucher = await _httpClient.GetAsync(UrlVoucher);
            string apiDataVoucher = await responeVoucher.Content.ReadAsStringAsync();
            var voucher = JsonConvert.DeserializeObject<Voucher>(apiDataVoucher);
            ViewBag.Voucher = voucher;
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

        [HttpPost] // Gửi
        public async Task<IActionResult> Checkout_saveBill(SaveBillViewModel ModelBill, decimal Total, int to_district_id, int to_ward_code)
        {
            // Authorize
            var UserId = Request.Cookies["UserID"];
            var Voucher_code = Request.Cookies["Voucher_id"];

            var UrlVoucher = $"https://localhost:7079/api/Voucher/GetVoucherByVoucherCode?VoucherCode={Voucher_code}";
            var responeVoucher = await _httpClient.GetAsync(UrlVoucher);
            string apiDataVoucher = await responeVoucher.Content.ReadAsStringAsync();
            var voucher = JsonConvert.DeserializeObject<Voucher>(apiDataVoucher);

            // đầu tiên cần check xem số lượng trong giỏ hàng có đủ trong db không
            int weight = 5000;
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
                    if (item.ComboID != null)
                    {
                        var UrlCheck = $"https://localhost:7079/api/Combo/CheckQuantity?ComboID={item.ComboID}&Quantity={item.Quantity}";
                        var contentCheck = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                        var responseCheck = await _httpClient.PostAsync(UrlCheck, contentCheck);

                        // Lấy sách ra để cộng thêm vào tổng cân nặng của sách

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

                    if (item.BookID != null)
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
            }

            // TÍnh giá tiền Ship :
            var feeShipUrl = $"https://localhost:7079/api/GhnApi/get-feeShip?to_district_id={to_district_id}&to_ward_code={to_ward_code}&weight={weight}";
            var responseFeeShip = await _httpClient.GetAsync(feeShipUrl);
            if (!responseFeeShip.IsSuccessStatusCode)
            {
                return BadRequest("Error getting shipping fee from GHN API");
            }
            var contentFeeShip = await responseFeeShip.Content.ReadAsStringAsync();
            var feeShipData = JsonConvert.DeserializeObject<feeShipGhnViewModel>(contentFeeShip);

            if (feeShipData == null && feeShipData.code != 200)
            {
                return BadRequest("Error getting shipping fee from GHN API");
            }
            decimal shippingFee = feeShipData.data.total;
            decimal? voucher_amount = voucher.DiscountAmount;
            // Kiểm tra nếu như điều kiện của Voucher không đúng thì phải báo lỗi :
            if (voucher.DiscountCondition > Total)
            {
                return BadRequest("Có vẻ như đơn hàng của bạn không đủ điều kiện sử dụng Voucher, vui lòng chọn mã Voucher khác");
            }
            // Tạo 1 Bill :
            var newBillId = Guid.NewGuid();
            Bill newBill = new Bill();
            newBill.BillID = newBillId;
            newBill.UserID = UserId;
            newBill.VoucherID = voucher.VoucherID;
            newBill.Email = ModelBill.Email;
            newBill.ReceiverName = ModelBill.firstName + " " + ModelBill.lastName;
            newBill.UserPhone = ModelBill.UserPhone;
            // Sử dụng giá trị tên của tỉnh, quận, và xã/phường
            newBill.AddressUser = ModelBill.ProvinceName + "," + ModelBill.DistrictName + "," + ModelBill.WardName;
            if (ModelBill.Street != null)
            {
                newBill.AddressUser += "," + ModelBill.Street;
            }
            newBill.Shipmoney = shippingFee;
            newBill.PriceBeforeVoucher = shippingFee + Total;
            if (voucher_amount == null)
            {
                newBill.Total = Total + shippingFee;
            }
            else
            {
                newBill.Total = Total + shippingFee - voucher_amount;
            }
            newBill.PaymentMethod = ModelBill.PaymentMethod;
            newBill.Status = 1;

            // Lưu Bill vào cơ sở dữ liệu :
            string urlBill;
            if (newBill.VoucherID == Guid.Parse("00000000-0000-0000-0000-000000000000") )
            {
                 urlBill = $"https://localhost:7079/api/Bill/CreateBillWithManualBillId?priceBeforeVoucher={newBill.PriceBeforeVoucher}&ReceiverName={newBill.ReceiverName}&Email={newBill.Email}&BillID={newBill.BillID}&UserID={newBill.UserID}&shipmoney={newBill.Shipmoney}&userPhone={newBill.UserPhone}&addressUser={newBill.AddressUser}&orderDate={DateTime.Now}&deliveryDate={DateTime.Today.AddDays(5)}&total={newBill.Total}&paymentMethod={newBill.PaymentMethod}&status={newBill.Status}";
            }
            else
            {
                urlBill = $"https://localhost:7079/api/Bill/CreateBillWithManualBillId?priceBeforeVoucher={newBill.PriceBeforeVoucher}&voucherID={newBill.VoucherID}&ReceiverName={newBill.ReceiverName}&Email={newBill.Email}&BillID={newBill.BillID}&UserID={newBill.UserID}&shipmoney={newBill.Shipmoney}&userPhone={newBill.UserPhone}&addressUser={newBill.AddressUser}&orderDate={DateTime.Now}&deliveryDate={DateTime.Today.AddDays(5)}&total={newBill.Total}&paymentMethod={newBill.PaymentMethod}&status={newBill.Status}";
            }
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
                        if (item.BookID != null)
                        {
                            var urlBook = $"https://localhost:7079/api/Book/GetBookByID/{item.BookID}";
                            var responseGetBookInfo = await _httpClient.GetAsync(urlBook);
                            string apiDataBookInfo = await responseGetBookInfo.Content.ReadAsStringAsync();
                            var bookInfo = JsonConvert.DeserializeObject<Book>(apiDataBookInfo);
                            // Tạo các BillItems từ các CartItem đã lưu :
                            BillItems billItems = new BillItems();
                            billItems.BillItemID = Guid.NewGuid();
                            billItems.BillID = newBillId;
                            billItems.BookID = item.BookID;
                            billItems.ItemName = item.ItemName;
                            billItems.Price = item.Price;
                            billItems.GiaNhap = bookInfo.EntryPrice;
                            billItems.Quantity = item.Quantity;
                            billItems.ToTal = billItems.Quantity * billItems.Price;
                            billItems.Status = 1;

                            // Sau khi tạo xong thì lưu nó vô database
                            var urlSaveBillItems = $"https://localhost:7079/api/BillItem/CreateBillItem?bookid={billItems.BookID}&billid={billItems.BillID}&itemname={billItems.ItemName}&price={billItems.Price}&quantity={billItems.Quantity}&total={billItems.ToTal}&giaNhap={billItems.GiaNhap}";
                            var contentBillItem = new StringContent(JsonConvert.SerializeObject(billItems), Encoding.UTF8, "application/json");
                            var responseCBIT = await _httpClient.PostAsync(urlSaveBillItems, contentBillItem);
                            if (!responseCBIT.IsSuccessStatusCode)
                            {
                                return BadRequest("Thêm không thành công");
                            }

                            // Cập nhật lại số lượng sản phẩm trong database
                            var urlUpdateQuantity = $"https://localhost:7079/api/Book/BuyBook?id={item.BookID}&quantityBuy={item.Quantity}";
                            var contentUpdateQuantity = new StringContent(JsonConvert.SerializeObject(billItems), Encoding.UTF8, "application/json");
                            var responseUpdateQuantity = await _httpClient.PutAsync(urlUpdateQuantity, contentUpdateQuantity);
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

                        if (item.ComboID != null)
                        {
                            var urlCombo = $"https://localhost:7079/api/Combo/GetAllCombo";
                            var responCombo = await _httpClient.GetAsync(urlCombo);
                            string apiDataCombo = await responCombo.Content.ReadAsStringAsync();
                            var lstCombo = JsonConvert.DeserializeObject<List<Combo>>(apiDataCombo);
                            Combo combo = lstCombo.FirstOrDefault(p => p.ComboID == item.ComboID);
                            // Tạo các BillItems từ các CartItem đã lưu :
                            BillItems billItems = new BillItems();
                            billItems.BillItemID = Guid.NewGuid();
                            billItems.BillID = newBillId;
                            billItems.ComboID = item.ComboID;
                            billItems.ItemName = item.ItemName;
                            billItems.Price = item.Price;
                            billItems.Quantity = item.Quantity;
                            billItems.ToTal = billItems.Quantity * billItems.Price;
                            billItems.Status = 1;

                            // Sau khi tạo xong thì lưu nó vô database
                            var urlSaveBillItems = $"https://localhost:7079/api/BillItem/CreateBillItem?comboid={billItems.ComboID}&billid={billItems.BillID}&itemname={billItems.ItemName}&price={billItems.Price}&quantity={billItems.Quantity}&total={billItems.ToTal}";
                            var contentBillItem = new StringContent(JsonConvert.SerializeObject(billItems), Encoding.UTF8, "application/json");
                            var responseCBIT = await _httpClient.PostAsync(urlSaveBillItems, contentBillItem);
                            if (!responseCBIT.IsSuccessStatusCode)
                            {
                                return BadRequest("Thêm không thành công");
                            }

                            // Cập nhật lại số lượng sản phẩm trong database
                            var urlUpdateQuantity = $"https://localhost:7079/api/Combo/BuyCombo?id={item.ComboID}&quantityBuy={item.Quantity}";
                            var contentUpdateQuantity = new StringContent(JsonConvert.SerializeObject(billItems), Encoding.UTF8, "application/json");
                            var responseUpdateQuantity = await _httpClient.PutAsync(urlUpdateQuantity, contentUpdateQuantity);
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
                }
                return RedirectToAction("GetAllBill","Bill");
            }
            
        }
        public async Task<IActionResult> FormCheckBill(Guid id)
        {
            var urlBill = $"https://localhost:7079/api/Bill/GetBillByBillId/{id}";
            var responeBill = await _httpClient.GetAsync(urlBill);
            string apiDataBill = await responeBill.Content.ReadAsStringAsync();
            Bill bill = JsonConvert.DeserializeObject<Bill>(apiDataBill);

            var urlBillItems = $"https://localhost:7079/api/BillItem/GetAllBillItemByBillID/{id}";
            var responBillItems = await _httpClient.GetAsync(urlBillItems);
            string apiDataBillItems = await responBillItems.Content.ReadAsStringAsync();
            List<BillItems> ListBillItems = JsonConvert.DeserializeObject<List<BillItems>>(apiDataBillItems);

            ViewBag.Bill = bill;
            ViewBag.BillItems = ListBillItems;
            return View();
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

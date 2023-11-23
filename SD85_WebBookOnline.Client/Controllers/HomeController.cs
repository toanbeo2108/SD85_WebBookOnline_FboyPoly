using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SD85_WebBookOnline.Client.Models;
using SD85_WebBookOnline.Share.ViewModels;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using SD85_WebBookOnline.Share.Models;
using Microsoft.AspNetCore.Identity;
namespace SD85_WebBookOnline.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly HttpClient _HttpClient;
        public List<CartItems> CartItemss { get; set; } = new List<CartItems>();
        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _HttpClient = new HttpClient();
        }

        public async Task<IActionResult> Index()
        {
            var urlBook = $"https://localhost:7079/api/Book/get-all-book";
            var responBook =  await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            if(lstBook == null)
            {
                return NotFound();
            }
            else
            {
                var lstBookOk = lstBook.Where(x => x.Status == 1).ToList();
                if(lstBookOk == null)
                {
                    return NotFound();
                }
                var lstSelectNew = lstBookOk.OrderByDescending(x => x.CreateDate).Take(6).ToList();
                ViewBag.lstSelectNew = lstSelectNew;
                var lstselectTopquantitysold = lstBookOk.OrderByDescending(x => x.QuantitySold).Take(8).ToList();
                ViewBag.lstTopquantitySold = lstselectTopquantitysold;
            }
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUser loginUser)
        {
            // Convert registerUser to JSON
            var loginUserJSON = JsonConvert.SerializeObject(loginUser);
            // Convert to string content
            var stringContent = new StringContent(loginUserJSON, Encoding.UTF8, "application/json");
            // Send request POST to register API
            var response = await _httpClient.PostAsync($"https://localhost:7079/api/login", stringContent);
            if (response.IsSuccessStatusCode)
            {
                string token = await response.Content.ReadAsStringAsync();
                Response.Cookies.Append("Token", token);
                // Tạo một đối tượng HttpRequestMessage.
                HttpRequestMessage request = new HttpRequestMessage();

                // Thêm token vào header của yêu cầu HTTP.
                request.Headers.Add("Authorization", $"Bearer {token}");

                // Gửi yêu cầu HTTP.
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Name).Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Role).Value));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                var check = User.Identity.IsAuthenticated;

                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                string jsonUserId = JsonConvert.SerializeObject(userId);
                Response.Cookies.Append("UserId", jsonUserId);
                return RedirectToAction("Index", "Home");

                //// Lấy UserID
                //var url = $"https://localhost:7079/api/user/GetUserId/{loginUser.Username}";
                //var respon = await _httpClient.GetAsync(url);
                //string api = await respon.Content.ReadAsStringAsync();
                //var UserId = JsonConvert.DeserializeObject<string>(api);
                //Response.Cookies.Append("UserID", UserId);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Message = await response.Content.ReadAsStringAsync();
                return View();
            }
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUser registerUser, string role)
        {
            // Convert registerUser to JSON
            var registerUserJSON = JsonConvert.SerializeObject(registerUser);

            // Convert to string content
            var stringContent = new StringContent(registerUserJSON, Encoding.UTF8, "application/json");

            // Add role to queryString
            role = "User";
            var queryString = $"?role={role}";

            // Send request POST to register API
            var response = await _httpClient.PostAsync($"https://localhost:7079/api/register{queryString}", stringContent);

            ViewBag.Message = await response.Content.ReadAsStringAsync();
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> deTail(Guid id)
        {
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
                if(lstImageBookDetail != null)
                {
                    ViewBag.lstImageBookDetail = lstImageBookDetail;
                }
                return View(Book);
            }
        }

        public async Task<IActionResult> AddToCart(Guid id, int quantity)
        {
            var urlBook = "https://localhost:7079/api/Book/get-all-book";
            var httpClient = new HttpClient();
            var responseBook = await httpClient.GetAsync(urlBook);
            if (!responseBook.IsSuccessStatusCode)
            {
                return BadRequest("Lỗi khi tải danh sách sách.");
            }
            string apiDataBook = await responseBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            ViewBag.lstBook = lstBook;
            var book = lstBook.FirstOrDefault(x => x.BookID == id);

            var urlCombo = $"https://localhost:7079/api/Combo/GetAllCombo";
            var responCombo = await _httpClient.GetAsync(urlCombo);
            string apiDataCombo = await responCombo.Content.ReadAsStringAsync();
            var lstCombo = JsonConvert.DeserializeObject<List<Combo>>(apiDataCombo);
            var combo = lstCombo.FirstOrDefault(x => x.ComboID == id);

            string json = Request.Cookies["myCart"];
            List<CartItems> myListCartItem = new List<CartItems>();
            if(json != null)
            {
                myListCartItem = JsonConvert.DeserializeObject<List<CartItems>>(json);
            }

            var existingItem = myListCartItem.FirstOrDefault(x => x.BookID == book.BookID);
            if (existingItem != null)
            {
                // Nếu sách đã có, tăng số lượng lên 1
                existingItem.Quantity += quantity;
                existingItem.ToTal = existingItem.Price * existingItem.Quantity;
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
                myListCartItem.Add(cartItems);
            }
            

            string updateJson = JsonConvert.SerializeObject(myListCartItem);
            Response.Cookies.Append("myCart", updateJson);
            return RedirectToAction("MyCart", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> MyCart()
        {
            var urlBook = "https://localhost:7079/api/Book/get-all-book";
            var httpClient = new HttpClient();
            var responseBook = await httpClient.GetAsync(urlBook);
            if (!responseBook.IsSuccessStatusCode)
            {
                return BadRequest("Lỗi khi tải danh sách sách.");
            }
            string apiDataBook = await responseBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            ViewBag.lstBook = lstBook;

            string json = Request.Cookies["myCart"];
            if(json != null)
            {
                List<CartItems> myListCartItem = JsonConvert.DeserializeObject<List<CartItems>>(json);
                ViewBag.myCart = myListCartItem;
                decimal subtotal = 0;
                foreach (var item in myListCartItem)
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
            }ViewBag.Subtotal = 0;
            
            return View();
        }
        public async Task<IActionResult> DeleteToCart(Guid id)
        {
            string json = Request.Cookies["myCart"];
            if (json != null)
            {
                List<CartItems> myList = JsonConvert.DeserializeObject<List<CartItems>>(json);

                // Tìm CartItems có ID tương ứng
                var item = myList.FirstOrDefault(x => x.BookID == id);
                if (item != null)
                {
                    // Xóa mục khỏi danh sách
                    myList.Remove(item);

                    // Chuyển đổi danh sách CartItems thành chuỗi JSON và lưu lại vào cookie
                    string updatedJson = JsonConvert.SerializeObject(myList);
                    Response.Cookies.Append("myCart", updatedJson);
                }
            }

            return RedirectToAction("MyCart", "Home");
        }
        [HttpGet]
        public async Task<IActionResult> Shop()
        {
            // Đọc cookie
            var urlBook = $"https://localhost:7079/api/Book/get-all-book";
            var responBook = await _httpClient.GetAsync(urlBook);
            string apiDataBook = await responBook.Content.ReadAsStringAsync();
            var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            if (lstBook == null)
            {
                return NotFound();
            }
            else
            {
                var lstBookOk = lstBook.Where(x => x.Status == 1).ToList();
                if (lstBookOk == null)
                {
                    return NotFound();
                }
                var lstSelect = lstBookOk.Take(18).ToList();
                ViewBag.lstSelect = lstSelect;
               
            }

            return Ok();
        }
        [HttpPost]

        public IActionResult Checkout()
        {
           
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
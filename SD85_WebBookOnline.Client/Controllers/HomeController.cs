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
        public int? TotalQuantitySold { get; set; } = 0;
        public decimal? DoanhThu { get; set; } = 0;
        public int? TotalQuantityPro { get; set; } = 0;
        public int TotalQuantityUser { get; set; } = 0;
        public string erro { get; set; }
        public int QuantityPro { get; set; } = 0;
        public List<Form> listForm { get; set; }
        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _HttpClient = new HttpClient();
            //SetDataProduct();
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var urlForm = "https://localhost:7079/api/Form/GetAllForm";
            var responForm = await _httpClient.GetAsync(urlForm);
            string apiDataForm = await responForm.Content.ReadAsStringAsync();
            var lstForm = JsonConvert.DeserializeObject<List<Form>>(apiDataForm);
            if (lstForm == null) { return NotFound("Không có  Hình thúc sách"); }
            listForm = lstForm.ToList();
            if (listForm == null)
            {
                erro = "Hình Thức Bị Null rồi";
            }
            var lstFormOk = JsonConvert.SerializeObject(listForm);
            Response.Cookies.Append("lstForm", lstFormOk);

			//
			string urlctd = $"https://localhost:7079/api/CategoryParent/GetAllCategoryParents";
			var responctd = await _httpClient.GetAsync(urlctd);
			string apidatactd = await responctd.Content.ReadAsStringAsync();
			var lstctd = JsonConvert.DeserializeObject<List<CategoryParent>>(apidatactd);
			if (lstctd == null)
			{
				erro = "Danh muc null";
			}
			var lstcatePOk = JsonConvert.SerializeObject(lstctd);
			Response.Cookies.Append("lstCateParent", lstcatePOk);
			//var lstCTPValue = Request.Cookies["lstCateParent"];
			//
			var urllanguage = $"https://localhost:7079/api/Languge/GetAllLanguge";
            var responlaguage = await _httpClient.GetAsync(urllanguage);
            string apiDatalanguae = await responlaguage.Content.ReadAsStringAsync();
            var lstLanguage = JsonConvert.DeserializeObject<List<Languge>>(apiDatalanguae);
            if (lstLanguage == null)
            {
                erro = "Danh muc null";
            }
            var lstlangOk = JsonConvert.SerializeObject(lstLanguage);
            Response.Cookies.Append("lstLanguage", lstlangOk);
            //
            var urlauthor = $"https://localhost:7079/api/Author/GetAllAuthor";
            var responauthor = await _httpClient.GetAsync(urlauthor);
            string apiDataauthor = await responauthor.Content.ReadAsStringAsync();
            var lstauthor = JsonConvert.DeserializeObject<List<Author>>(apiDataauthor);
            if (lstauthor == null)
            {
                erro = "Danh muc null";
            }
            var lstAuthorOk = JsonConvert.SerializeObject(lstauthor);
            Response.Cookies.Append("lstAuthor", lstAuthorOk);
            //
            var urlCate = $"https://localhost:7079/api/Category/GetAllCategory";
            var responCate = await _httpClient.GetAsync(urlCate);
            string apiDataCate = await responCate.Content.ReadAsStringAsync();
            var lstCate = JsonConvert.DeserializeObject<List<Category>>(apiDataCate);
            if (lstCate == null)
            {
                erro = "null roi";
            }
            var lstCateOk = JsonConvert.SerializeObject(lstCate);
            Response.Cookies.Append("lstCategory", lstCateOk);
            //
            var urlManufacturer = $"https://localhost:7079/api/Manufacturer/GetAllManufacturer";
            var responManufacturer = await _httpClient.GetAsync(urlManufacturer);
            string apiDataManufacturer = await responManufacturer.Content.ReadAsStringAsync();
            var lstManufacturer = JsonConvert.DeserializeObject<List<Manufacturer>>(apiDataManufacturer);
            if (lstManufacturer == null)
            {
                erro = "null roi kia";
            }
            var lstManuOk = JsonConvert.SerializeObject(lstManufacturer);
            Response.Cookies.Append("lstManu", lstManuOk);
            //
            var urlCombo = $"https://localhost:7079/api/Combo/GetAllCombo";
            var responCombo = await _httpClient.GetAsync(urlCombo);
            string apiDataCombo = await responCombo.Content.ReadAsStringAsync();
            var lstCombo = JsonConvert.DeserializeObject<List<Combo>>(apiDataCombo);
            ViewBag.listCombo = lstCombo;

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
                foreach(var item in lstBook)
                {
                    TotalQuantityPro += item.TotalQuantity;
                    TotalQuantitySold += item.QuantitySold; 
                }
                //
                var lstBookOk = lstBook.Where(x => x.Status == 1).ToList();
                if (lstBookOk == null)
                {
                    return NotFound();
                }
                var lstSelectNew = lstBookOk.OrderByDescending(x => x.CreateDate).Take(6).ToList();
                ViewBag.lstSelectNew = lstSelectNew;
                var lstselectTopquantitysold = lstBookOk.OrderByDescending(x => x.QuantitySold).Take(8).ToList();
                ViewBag.lstTopquantitySold = lstselectTopquantitysold;
            }

            var totalQuantityPro = JsonConvert.SerializeObject(TotalQuantityPro);
            Response.Cookies.Append("ToTalQuantityPro", totalQuantityPro);

            var totalQuantitySold = JsonConvert.SerializeObject(TotalQuantitySold);
            Response.Cookies.Append("totalQuantitySold", totalQuantitySold);

            //
        
            var url = $"https://localhost:7079/api/user/GetUsersByRole?roleName=User";
            var response = await _httpClient.GetAsync(url);
            string apiDataUser = await response.Content.ReadAsStringAsync();
            var ListUser = JsonConvert.DeserializeObject<List<User>>(apiDataUser);
            if (ListUser != null)
            {
                TotalQuantityUser += ListUser.Count();
            }
            var valueTotalqttUser = JsonConvert.SerializeObject(TotalQuantityUser);
            Response.Cookies.Append("ToTalQuantityUser", valueTotalqttUser);
            //
            var Url = $"https://localhost:7079/api/Bill/GetAllBill";
            var respone = await _httpClient.GetAsync(Url);
            string apiData = await respone.Content.ReadAsStringAsync();
            var Bills = JsonConvert.DeserializeObject<List<Bill>>(apiData);
            if(Bills == null)
            {
                return NotFound();
            }
            var BillOk = Bills.Where(x => x.Status == 3);
            foreach(var item in BillOk)
            {
                DoanhThu += item.Total;
            }
            var cookieValueDanhthu = JsonConvert.SerializeObject(Convert.ToInt32(DoanhThu));
            Response.Cookies.Append("ToTalDanhThu", cookieValueDanhthu);
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

                // Lấy UserID
                var url = $"https://localhost:7079/api/user/GetUserId/{loginUser.Username}";
                var respon = await _httpClient.GetAsync(url);
                if (respon.IsSuccessStatusCode)
                {
                    var api = await respon.Content.ReadAsStringAsync();
                    var UserId = api; // Gán trực tiếp chuỗi nhận được từ API cho UserId
                    Response.Cookies.Append("UserID", UserId);

                    var urlCart = $"https://localhost:7079/api/Cart/GetCartByIdUser/{UserId}?status=1";
                    var responCart = await _httpClient.GetAsync(urlCart);
                    string apiDataCart = await responCart.Content.ReadAsStringAsync();
                    var ListCart = JsonConvert.DeserializeObject<List<Cart>>(apiDataCart);
                    //Lấy toàn bộ Cart có UserId = Userid truyền đến
                    if (ListCart != null)
                    {
                        foreach (var Cart in ListCart)
                        {
                            var urlCartItems = $"https://localhost:7079/api/CartItem/GetCartItemByCartID/{Cart.CartId}";
                            var responCartItems = await _httpClient.GetAsync(urlCartItems);
                            string apiDataCartItems = await responCartItems.Content.ReadAsStringAsync();
                            List<CartItems> ListCartItems = JsonConvert.DeserializeObject<List<CartItems>>(apiDataCartItems);
                            if (ListCartItems != null)
                            {
                                QuantityPro = ListCartItems.Count;
                                Response.Cookies.Append("QuantityPro", QuantityPro.ToString());
                            }
                        }

                        ViewBag.QuantityPro = QuantityPro;
                    }
                }
                else
                {
                    // Xử lý lỗi nếu có
                    _logger.LogError($"Error calling API. Status code: {respon.StatusCode}");
                }

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
            if (json != null)
            {
                myListCartItem = JsonConvert.DeserializeObject<List<CartItems>>(json);
            }

            var existingItem = myListCartItem.FirstOrDefault(x => x.BookID == id || x.ComboID == id);
            if (existingItem != null)
            {
                // Nếu sách đã có, tăng số lượng lên
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
            if (json != null)
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
            }
            ViewBag.Subtotal = 0;

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

        [HttpPost]

        public IActionResult Checkout()
        {

            return View();
        }
        

        [HttpGet]
        public async void SetDataProduct()
        {
           
            //var urlBook = $"https://localhost:7079/api/Book/get-all-book";
            //var responBook = await _httpClient.GetAsync(urlBook);
            //string apiDataBook = await responBook.Content.ReadAsStringAsync();
            //var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            //if (lstBook != null)
            //{
            //    foreach (var item in lstBook)
            //    {
            //        TotalQuantityPro += item.TotalQuantity;
            //        if (item.QuantityExists == 0)
            //        {
            //            item.Status = 0;
            //        }
            //    }
            //    var valueTotalqttPro = JsonConvert.SerializeObject(TotalQuantityPro);
            //    Response.Cookies.Append("ToTalQuantityBook", valueTotalqttPro);
            //}


            //var token = Request.Cookies["Token"];
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //var url = $"https://localhost:7079/api/user/GetUsersByRole?roleName=User";
            //var response = await _httpClient.GetAsync(url);
            //string apiDataUser = await response.Content.ReadAsStringAsync();
            //var ListUser = JsonConvert.DeserializeObject<List<User>>(apiDataUser);
            //if(ListUser != null)
            //{
            //    TotalQuantityUser += ListUser.Count();
            //}
            //var valueTotalqttUser = JsonConvert.SerializeObject(TotalQuantityUser);
            //Response.Cookies.Append("ToTalQuantityUser",valueTotalqttUser);


            //var urlCombo = $"https://localhost:7079/api/Combo/GetAllCombo";
            //var responCombo = await _httpClient.GetAsync(urlCombo);
            //string apiDataCombo = await responCombo.Content.ReadAsStringAsync();
            //var lstCombo = JsonConvert.DeserializeObject<List<Combo>>(apiDataCombo);
            //if(lstCombo != null )
            //{
            //    foreach(var item in lstCombo)
            //    {
            //        if(item.Quantity == 0)
            //        {
            //            item.Status = 0;
            //        }
            //    }
            //}
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
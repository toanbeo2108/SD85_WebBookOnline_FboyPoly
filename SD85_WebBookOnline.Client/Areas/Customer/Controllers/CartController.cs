using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Client.Controllers;
using SD85_WebBookOnline.Share.Models;
using System.Text;

namespace SD85_WebBookOnline.Client.Areas.Customer.Controllers
{
    public class CartController : Controller
    {
        private readonly HttpClient _httpClient;
        public List<CartItems> CartItemss { get; set; } = new List<CartItems>();
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
            return RedirectToAction("MyCart", "Cart");
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

            return View();
        }


        //[HttpPost]
        //public async Task<IActionResult> Checkout()
        //{
            //var urlBook = "https://localhost:7079/api/Book/get-all-book";
            //var httpClient = new HttpClient();
            //var responseBook = await httpClient.GetAsync(urlBook);
            //if (!responseBook.IsSuccessStatusCode)
            //{
            //    return BadRequest("Lỗi khi tải danh sách sách.");
            //}
            //string apiDataBook = await responseBook.Content.ReadAsStringAsync();
            //var lstBook = JsonConvert.DeserializeObject<List<Book>>(apiDataBook);
            //decimal allPrice = 0;
            //string json = Request.Cookies["myCart"];
            //if (json != null)
            //{
            //    List<CartItems> myListCartItem = JsonConvert.DeserializeObject<List<CartItems>>(json);
            //    foreach (var item in myListCartItem)
            //    {
            //        allPrice = allPrice + item.ToTal;
            //    }
            //}
           
            //string UserId = Request.Cookies["UserId"];
            //if(UserId != null)
            //{
            //    Cart cart = new Cart();
            //    cart.CartId = Guid.NewGuid();
            //    cart.UserID = UserId;
            //    cart.VoucherID = null;
            //    cart.PriceBeforeVoucher = allPrice;
            //    cart.Total = allPrice;
            //    cart.Status = 1;
            //    string urlCreateCart = $"https://localhost:7079/api/Cart/CreateCart?CartId={cart.CartId}&UserId={cart.UserID}&priceBeforeVoucher={cart.PriceBeforeVoucher}&total={cart.Total}";
            //    var content = new StringContent(JsonConvert.SerializeObject(cart), Encoding.UTF8, "application/json");
            //    var respon = await _httpClient.PostAsync(urlCreateCart, content);
            //    if (respon.IsSuccessStatusCode)
            //    {
            //        string jsonitem = Request.Cookies["myCart"];
            //        if (json != null)
            //        {
            //            List<CartItems> myListCartItem = JsonConvert.DeserializeObject<List<CartItems>>(jsonitem);
            //            foreach (var item in myListCartItem)
            //            {
            //                string urlcartItem = $"https://localhost:7079/api/CartItem/Add-CartItem?CartID={cart.CartId}&BookID={item.BookID}&image={item.Image}&ItemName={item.ItemName}&Price={item.Price}&Quantity={item.Quantity}&ToTal={item.ToTal}&Status={1}";
            //                var contentCIT = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
            //                var responCIT = await _httpClient.PostAsync(urlcartItem, contentCIT);
            //                if(responCIT.IsSuccessStatusCode)
            //                {
            //                    return View();
            //                }
            //                return BadRequest();
            //            }
            //        }
            //        return View();
            //    }
            //    return BadRequest();
            //}
            

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

            return RedirectToAction("MyCart", "Cart");
        }
        public IActionResult Checkout()
        {


            return View();
        }

    }
}

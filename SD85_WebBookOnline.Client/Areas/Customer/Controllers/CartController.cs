﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Client.Controllers;
using SD85_WebBookOnline.Share.Models;

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
        public async Task<IActionResult> AddToCart(Guid id)
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
                cartItems.Quantity = 1;
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
                cartItems.Quantity = 1;
                cartItems.ToTal = cartItems.Price * cartItems.Quantity;
                cartItems.Status = 1;
            }

            myListCartItem.Add(cartItems);
            string updateJson = JsonConvert.SerializeObject(myListCartItem);
            Response.Cookies.Append("myCart", updateJson);
            return RedirectToAction("MyCart", "Cart",new {area = "Customer"});
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
            }
            return View();
        }
        public IActionResult Checkout()
        {
            return View();
        }

    }
}
﻿using Microsoft.AspNetCore.Authentication.Cookies;
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

namespace SD85_WebBookOnline.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        private readonly HttpClient _HttpClient;
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
            

            //var urlCombo = $"https://localhost:7079/api/Combo/GetAllCombo";
            //var httpClient = new HttpClient();
            //var responCombo = await _HttpClient.GetAsync(urlCombo);
            //string apiDataCombo = await responCombo.Content.ReadAsStringAsync();
            //var lstCombo = JsonConvert.DeserializeObject<List<Combo>>(apiDataCombo);
            //if(lstCombo == null)
            //{
            //    return NotFound() ;
            //}
            //var lstselectcombotop = lstCombo.OrderByDescending(x => x.Price).Take(6).ToList();
            //ViewBag.lstComboTop = lstselectcombotop;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
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

namespace SD85_WebBookOnline.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public IActionResult Index()
        {
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


                return RedirectToAction("Index", "AdminHome",new {area = "Admin"});

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
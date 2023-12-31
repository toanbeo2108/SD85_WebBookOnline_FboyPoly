﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SD85_WebBookOnline.Client.Areas.Customer.Controllers
{
    public class VoucherController : Controller
    {
        private readonly HttpClient _httpClient;
        public VoucherController()
        {
            _httpClient = new HttpClient();
        }
        

        [HttpGet]
        public async Task<IActionResult> DSVoucher()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlVoucher = $"https://localhost:7079/api/Voucher/GetAllVoucher";
            var responVoucher = await _httpClient.GetAsync(urlVoucher);
            string apiDataVoucher = await responVoucher.Content.ReadAsStringAsync();
            var lstVoucher = JsonConvert.DeserializeObject<IEnumerable<Voucher>>(apiDataVoucher);
         
            ViewBag.lstVoucher = lstVoucher;
            return View();
        }

    }
}

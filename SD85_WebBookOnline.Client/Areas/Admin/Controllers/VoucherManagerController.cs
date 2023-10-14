﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Text;

namespace SD85_WebBookOnline.Client.Areas.Admin.Controllers
{
    public class VoucherManagerController : Controller
    {
        private readonly HttpClient _httpClient;
        public VoucherManagerController()
        {
            _httpClient = new HttpClient();
        }
        public IActionResult Index()
        {
            return View();
        }
       
        public async Task<IActionResult> AllVoucherManager()
        {
            var urlVoucher = $"https://localhost:7079/api/Voucher/GetAllVoucher";
            var responVoucher = await _httpClient.GetAsync(urlVoucher);
            string apiDataVoucher = await responVoucher.Content.ReadAsStringAsync();
            var lstVoucher = JsonConvert.DeserializeObject<List<Voucher>>(apiDataVoucher);
            return View(lstVoucher);
        }
        public IActionResult CreateVoucher()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateVoucher(Voucher vc)
        {
            var urlVoucher = $"https://localhost:7079/api/Voucher/CreateVoucher";
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8,"application/json");
            var respon = await _httpClient.PostAsync(urlVoucher, content);
            if (respon.IsSuccessStatusCode)
            {
                RedirectToAction("AllVoucherManager", "VoucherAdmin",new {area = "Admin"});
            }
            TempData["Erro Message"] = "Thêm Thất Bại";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> VoucherDetail(Guid id)
        {
            var urlVoucher = $"https://localhost:7079/api/Voucher/GetAllVoucher";
            var responVoucher = await _httpClient.GetAsync(urlVoucher);
            string apiDataVoucher = await responVoucher.Content.ReadAsStringAsync();
            var lstVoucher = JsonConvert.DeserializeObject<List<Voucher>>(apiDataVoucher);
            var voucher = lstVoucher.FirstOrDefault(x => x.VoucherID == id);
            if(voucher == null)
            {
                return BadRequest();
            }
            else
            {
                return View(voucher);   
            }
        }
        public async Task<IActionResult> UpdateVoucher(Guid id)
        {
            var urlVoucher = $"https://localhost:7079/api/Voucher/GetAllVoucher";
            var responVoucher = await _httpClient.GetAsync(urlVoucher);
            string apiDataVoucher = await responVoucher.Content.ReadAsStringAsync();
            var lstVoucher = JsonConvert.DeserializeObject<List<Voucher>>(apiDataVoucher);
            var voucher = lstVoucher.FirstOrDefault(x => x.VoucherID == id);
            if (voucher == null)
            {
                return BadRequest();
            }
            else
            {
                return View(voucher);
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateVoucher(Guid id,Voucher vc)
        {
            var urlVoucher = $"https://localhost:7079/api/Voucher/UpdateVoucher/{id}";
            var content = new StringContent(JsonConvert.SerializeObject(vc), Encoding.UTF8, "application/json");
            var respon =  await _httpClient.PutAsync(urlVoucher, content);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            return RedirectToAction("AllVoucherManager", "VoucherAdmin", new { area = "Admin" });

        }
        [HttpGet]
        public async Task<IActionResult> DeleteVoucher(Guid id)
        {
            var urlVoucher = $"https://localhost:7079/api/Voucher/DeleteVoucher/{id}";
            var respon =  await _httpClient.DeleteAsync(urlVoucher);
            if (!respon.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            return RedirectToAction("AllVoucherManager", "VoucherAdmin", new { area = "Admin" });
        }

    }
}

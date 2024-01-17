﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.Server;
using Newtonsoft.Json;
using SD85_WebBookOnline.Share.Models;
using System.Net.Http.Headers;

namespace SD85_WebBookOnline.Client.Areas.Customer.Controllers
{
    public class ShopController : Controller
    {
        private readonly HttpClient _httpClient;
        public ShopController()
        {
            _httpClient = new HttpClient();
        }
        [HttpGet]
        public async Task<IActionResult> All_Pro()
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

            var urlCombo = $"https://localhost:7079/api/Combo/GetAllCombo";
            var httpClient = new HttpClient();
            var responCombo = await _httpClient.GetAsync(urlCombo);
            string apiDataCombo = await responCombo.Content.ReadAsStringAsync();
            var lstCombo = JsonConvert.DeserializeObject<List<Combo>>(apiDataCombo);
            if (lstCombo == null)
            {
                return NotFound();
            }
            else
            {
                var lstComboOk = lstCombo.Where(x => x.Status == 1).ToList();
                if (lstComboOk == null)
                {
                    return NotFound();
                }
                var lstcomboSelectdown100 = lstComboOk.Take(18).ToList();
                ViewBag.lstComboSelect = lstcomboSelectdown100;
            }
            return View();
        }
        #region Price
        public async Task<IActionResult> usePricedown_100()
        {
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
                var lstBookcheck = lstBookOk.Where(x => x.Price >= 0 && x.Price <= 100000).ToList();
                var lstbookSelectdown100 = lstBookcheck.Take(18).ToList();
                ViewBag.lstbookSelect = lstbookSelectdown100;

            }
            var urlCombo = $"https://localhost:7079/api/Combo/GetAllCombo";
            var httpClient = new HttpClient();
            var responCombo = await _httpClient.GetAsync(urlCombo);
            string apiDataCombo = await responCombo.Content.ReadAsStringAsync();
            var lstCombo = JsonConvert.DeserializeObject<List<Combo>>(apiDataCombo);
            if (lstCombo == null)
            {
                return NotFound();
            }
            else
            {
                var lstComboOk = lstCombo.Where(x => x.Status == 1).ToList();
                if (lstComboOk == null)
                {
                    return NotFound();
                }
                var lstComboCheck = lstComboOk.Where(x => x.Price >= 0 && x.Price <= 100000).ToList();
                var lstcomboSelectdown100 = lstComboCheck.Take(18).ToList();
                ViewBag.lstComboSelect = lstcomboSelectdown100;
            }
            return View();
        }
        public async Task<IActionResult> usePricebetween_100to200()
        {
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
                var lstBookcheck = lstBookOk.Where(x => x.Price > 100000 && x.Price <= 200000).ToList();
                var lstSelectbt100to200 = lstBookcheck.Take(18).ToList();
                ViewBag.lstSelect = lstSelectbt100to200;

            }
            var urlCombo = $"https://localhost:7079/api/Combo/GetAllCombo";
            var httpClient = new HttpClient();
            var responCombo = await _httpClient.GetAsync(urlCombo);
            string apiDataCombo = await responCombo.Content.ReadAsStringAsync();
            var lstCombo = JsonConvert.DeserializeObject<List<Combo>>(apiDataCombo);
            if (lstCombo == null)
            {
                return NotFound();
            }
            else
            {
                var lstComboOk = lstCombo.Where(x => x.Status == 1).ToList();
                if (lstComboOk == null)
                {
                    return NotFound();
                }
                var lstComboCheck = lstComboOk.Where(x => x.Price > 100000 && x.Price <= 200000).ToList();
                var lstconboSelectbt100to200 = lstComboCheck.Take(18).ToList();
                ViewBag.lstComboSelect = lstconboSelectbt100to200;
            }
            return View();
        }

        public async Task<IActionResult> usePricebetween_200to400()
        {
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
                var lstBookcheck = lstBookOk.Where(x => x.Price > 200000 && x.Price <= 400000).ToList();
                var lstSelectbt200to400 = lstBookcheck.Take(18).ToList();
                ViewBag.lstSelect = lstSelectbt200to400;

            }
            var urlCombo = $"https://localhost:7079/api/Combo/GetAllCombo";
            var httpClient = new HttpClient();
            var responCombo = await _httpClient.GetAsync(urlCombo);
            string apiDataCombo = await responCombo.Content.ReadAsStringAsync();
            var lstCombo = JsonConvert.DeserializeObject<List<Combo>>(apiDataCombo);
            if (lstCombo == null)
            {
                return NotFound();
            }
            else
            {
                var lstComboOk = lstCombo.Where(x => x.Status == 1).ToList();
                if (lstComboOk == null)
                {
                    return NotFound();
                }
                var lstComboCheck = lstComboOk.Where(x => x.Price > 200000 && x.Price <= 400000).ToList();
                var lstconboSelectbt200to400 = lstComboCheck.Take(18).ToList();
                ViewBag.lstComboSelect = lstconboSelectbt200to400;
            }
            return View();
        }

        public async Task<IActionResult> usePricebetween_400to500()
        {
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
                var lstBookcheck = lstBookOk.Where(x => x.Price > 400000 && x.Price <= 500000).ToList();
                var lstSelectbt200to400 = lstBookcheck.Take(18).ToList();
                ViewBag.lstSelect = lstSelectbt200to400;

            }
            var urlCombo = $"https://localhost:7079/api/Combo/GetAllCombo";
            var httpClient = new HttpClient();
            var responCombo = await _httpClient.GetAsync(urlCombo);
            string apiDataCombo = await responCombo.Content.ReadAsStringAsync();
            var lstCombo = JsonConvert.DeserializeObject<List<Combo>>(apiDataCombo);
            if (lstCombo == null)
            {
                return NotFound();
            }
            else
            {
                var lstComboOk = lstCombo.Where(x => x.Status == 1).ToList();
                if (lstComboOk == null)
                {
                    return NotFound();
                }
                var lstComboCheck = lstComboOk.Where(x => x.Price > 400000 && x.Price <= 500000).ToList();
                var lstconboSelectbt200to400 = lstComboCheck.Take(18).ToList();
                ViewBag.lstComboSelect = lstconboSelectbt200to400;
            }
            return View();
        }

        public async Task<IActionResult> usePrice_500Up()
        {
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
                var lstBookcheck = lstBookOk.Where(x => x.Price > 500000).ToList();
                var lstSelect500up = lstBookcheck.Take(18).ToList();
                ViewBag.lstSelect = lstSelect500up;

            }
            var urlCombo = $"https://localhost:7079/api/Combo/GetAllCombo";
            var httpClient = new HttpClient();
            var responCombo = await _httpClient.GetAsync(urlCombo);
            string apiDataCombo = await responCombo.Content.ReadAsStringAsync();
            var lstCombo = JsonConvert.DeserializeObject<List<Combo>>(apiDataCombo);
            if (lstCombo == null)
            {
                return NotFound();
            }
            else
            {
                var lstComboOk = lstCombo.Where(x => x.Status == 1).ToList();
                if (lstComboOk == null)
                {
                    return NotFound();
                }
                var lstComboCheck = lstComboOk.Where(x => x.Price > 500000).ToList();
                var lstconboSelect500up = lstComboCheck.Take(18).ToList();
                ViewBag.lstComboSelect = lstconboSelect500up;
            }
            return View();
        }
        #endregion //


        #region Form
        public async Task<IActionResult> FilterBy_Form(Guid formid)
        {
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
                var lstBookFilterByForm = lstBookOk.Where(x => x.FormID == formid).Take(18).ToList();
                if (lstBookFilterByForm == null)
                {
                    return NotFound();
                }
                ViewBag.lstBookFilterByForm = lstBookFilterByForm;
            }
            return View();
        }
        #endregion


        #region Author
        public async Task<IActionResult> FilterBy_Author(Guid authorID)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            var urlBookDetail = $"https://localhost:7079/api/BookDetail/GetAllBookDetail";
            var responBD = await _httpClient.GetAsync(urlBookDetail);
            string apiDataBD = await responBD.Content.ReadAsStringAsync();
            var lstBD = JsonConvert.DeserializeObject<List<BookDetail>>(apiDataBD); // lấy ds các BookDetail
            if (lstBD == null)
            {
                return NotFound("Null rồi");
            }
            var lstBDFilter = lstBD.Where(x => x.AuthorID == authorID).ToList();
            if (lstBDFilter == null)
            {
                return NotFound("BD Null Đoạn này");
            }

            List<Book> lstBookFilterAuthor = new List<Book>();
            //LẤY ds sách
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
                foreach (var item1 in lstBookOk)
                {
                    foreach (var item2 in lstBDFilter)
                    {
                        if (item1.BookID == item2.BookID)
                        {
                            lstBookFilterAuthor.Add(item1);
                        }
                    }
                }

                ViewBag.lstBookFilterAuthor = lstBookFilterAuthor;
            }
            return View();
        }
        #endregion


        #region category
        public async Task<IActionResult> FilterBy_Category(Guid categoryID)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            var urlBookDetail = $"https://localhost:7079/api/BookDetail/GetAllBookDetail";
            var responBD = await _httpClient.GetAsync(urlBookDetail);
            string apiDataBD = await responBD.Content.ReadAsStringAsync();
            var lstBD = JsonConvert.DeserializeObject<List<BookDetail>>(apiDataBD); // lấy ds các BookDetail
            if (lstBD == null)
            {
                return NotFound("Null rồi");
            }
            var lstBDFilter = lstBD.Where(x => x.CategoriesID == categoryID).ToList();
            if (lstBDFilter == null)
            {
                return NotFound("BD Null Đoạn này");
            }

            List<Book> lstBookFilterCategory = new List<Book>();
            //LẤY ds sách
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
                foreach (var item1 in lstBookOk)
                {
                    foreach (var item2 in lstBDFilter)
                    {
                        if (item1.BookID == item2.BookID)
                        {
                            lstBookFilterCategory.Add(item1);
                        }
                    }
                }

                ViewBag.lstBookFilterAuthor = lstBookFilterCategory;
            }
            return View();
        }
        #endregion


        #region Language
        public async Task<IActionResult> FilterBy_Language(Guid languageID)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


            var urlBookDetail = $"https://localhost:7079/api/BookDetail/GetAllBookDetail";
            var responBD = await _httpClient.GetAsync(urlBookDetail);
            string apiDataBD = await responBD.Content.ReadAsStringAsync();
            var lstBD = JsonConvert.DeserializeObject<List<BookDetail>>(apiDataBD); // lấy ds các BookDetail
            if (lstBD == null)
            {
                return NotFound("Null rồi");
            }
            var lstBDFilter = lstBD.Where(x => x.LagugeID == languageID).ToList();
            if (lstBDFilter == null)
            {
                return NotFound("BD Null Đoạn này");
            }

            List<Book> lstBookFilterLanguage = new List<Book>();
            //LẤY ds sách
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
                foreach (var item1 in lstBookOk)
                {
                    foreach (var item2 in lstBDFilter)
                    {
                        if (item1.BookID == item2.BookID)
                        {
                            lstBookFilterLanguage.Add(item1);
                        }
                    }
                }

                ViewBag.lstBookFilterAuthor = lstBookFilterLanguage;
            }
            return View();
        }
        #endregion


        #region Manufacture
        public async Task<IActionResult> FilterBy_Manufacture(Guid manuID)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);



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
                var lstFilterManu = lstBookOk.Where(x => x.ManufacturerID == manuID).Take(18).ToList();

                ViewBag.lstFilterManu = lstFilterManu;
            }
            return View();
        }
        #endregion

        #region CategoryParent
        public async Task<IActionResult> FilterBy_CategoryParent(Guid categoryparentID)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var urlcategoryP = $"https://localhost:7079/api/CategoryParent/GetAllCategoryParents";
            var responcategoryP = await _httpClient.GetAsync(urlcategoryP);
            string apiDataCateP = await responcategoryP.Content.ReadAsStringAsync();
            var lstCateP = JsonConvert.DeserializeObject<List<CategoryParent>>(apiDataCateP);
            if (lstCateP == null)
            {
                return NotFound("Không tồn tại thể loại cha nào của sách");
            }
            var CateOk = lstCateP.FirstOrDefault(x => x.CategoryParentID == categoryparentID); // lấy cate cha thỏa mãn 
            if (CateOk == null)
            {
                return NotFound("Không tồn tại Danh Mục sách này");
            }
            var urlcategory = $"https://localhost:7079/api/Category/GetAllCategory";
            var responcategory = await _httpClient.GetAsync(urlcategory);
            string apiDataCate = await responcategory.Content.ReadAsStringAsync();
            var lstCate = JsonConvert.DeserializeObject<List<Category>>(apiDataCate);
            if (lstCate == null)
            {
                return NotFound("Không tồn tại thể loại nào của sách");
            }

            var lstCateOk = lstCate.Where(x => x.CategoryParentID == CateOk.CategoryParentID).ToList();
            List<BookDetail> lstBDFilter = new List<BookDetail>();
            var urlBookDetail = $"https://localhost:7079/api/BookDetail/GetAllBookDetail";
            var responBD = await _httpClient.GetAsync(urlBookDetail);
            string apiDataBD = await responBD.Content.ReadAsStringAsync();
            var lstBD = JsonConvert.DeserializeObject<List<BookDetail>>(apiDataBD); // lấy ds các BookDetail
            if (lstBD == null)
            {
                return NotFound("Null rồi");
            }

            foreach (var itemCate in lstCateOk)
            {
                foreach (var itemBD in lstBD)
                {
                    if (itemCate.CategoryID == itemBD.CategoriesID)
                    {
                        lstBDFilter.Add(itemBD);
                    }
                }
            }
            List<Book> lstBookFilterCategoryParent = new List<Book>();
            //LẤY ds sách
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
                foreach (var item1 in lstBookOk)
                {
                    foreach (var item2 in lstBDFilter)
                    {
                        if (item1.BookID == item2.BookID)
                        {
                            lstBookFilterCategoryParent.Add(item1);
                        }
                    }
                }

                ViewBag.lstBookFilterCategoryParent = lstBookFilterCategoryParent;
            }
            return View();
        }
        #endregion
    }
}

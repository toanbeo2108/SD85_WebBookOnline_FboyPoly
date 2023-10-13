﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;

namespace SD85_WebBookOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IAllResponsitories<Book> ires;
         AppDbContext context = new AppDbContext();
        public BookController()
        {
            ires = new AllResponsitories<Book>(context, context.Book);
        }
        [HttpGet("get-all-book")]
        public async Task<IEnumerable<Book>> GetAllbook()
        {
            return await ires.GetAll();
        }
        [HttpPost("add-book")]
        public async Task<bool> addbook(Guid ManufacturerID,Guid FormID, Guid CouponID, string BookName, int TotalQuantity, string MainPhoto, int QuantitySold, int QuantityExists, decimal EntryPrice, string Information, string Description, string ISBN, int YearOfRelease, DateTime CreateDate, DateTime DeleteDate, int TransactionStatus, int Status)
        {

                Book b = new Book();
                b.BookID = Guid.NewGuid();
                b.ManufacturerID = ManufacturerID;
                b.FormID = FormID;
                b.CouponID = CouponID;
                b.BookName = BookName;
                b.TotalQuantity = TotalQuantity;
                b.MainPhoto = MainPhoto;
                b.QuantitySold = QuantitySold;
                b.QuantityExists = QuantityExists;
                b.EntryPrice = EntryPrice;
                b.Information = Information;
                b.Description = Description;
                b.ISBN = ISBN;
                b.YearOfRelease = YearOfRelease;
                b.CreateDate = CreateDate;
                b.DeleteDate = DeleteDate;
                b.TransactionStatus = TransactionStatus;
                b.Status = Status;
                return await ires.CreateItem(b);

        }
        [HttpDelete("delete-book/{id}")]
        public async Task<bool> deleteBook(Guid id)
        {
            var listBook = await ires.GetAll();
            var re = listBook.FirstOrDefault(c => c.BookID == id);
            if (re != null)
            {
                return await ires.DeleteItem(re);
            }
            else
            {
                return false;
            }
        }
        [HttpPut("updat-Book/{id}")]
        public async Task<bool> UpdateBook(Guid id, [FromBody]Book book)
        {
            var listBook = await ires.GetAll();
            var b = listBook.FirstOrDefault(c => c.BookID == id);
            if (b != null)
            {
                
                b.ManufacturerID = book.ManufacturerID;
                b.FormID = book.FormID;
                b.CouponID = book.CouponID;
                b.BookName = book.BookName;
                b.TotalQuantity = book.TotalQuantity;
                b.MainPhoto = book.MainPhoto;
                b.QuantitySold = book.QuantitySold;
                b.QuantityExists = book.QuantityExists;
                b.EntryPrice = book.EntryPrice;
                b.Information = book.Information;
                b.Description = book.Description;
                b.ISBN = book.ISBN;
                b.YearOfRelease = book.YearOfRelease;
                b.CreateDate = book.CreateDate;
                b.DeleteDate = book.DeleteDate;
                b.TransactionStatus = book.TransactionStatus;
                b.Status = book.Status;
                return await ires.UpdateItem(b);
            }
            else
            {
                return false;
            }
        }
    }
}
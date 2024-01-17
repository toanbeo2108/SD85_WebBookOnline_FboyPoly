using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;
using System.Data;

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
        [HttpGet("GetBookByID/{id}")]
        public async Task<Book> GetBookById(Guid id)
        {
            return await ires.GetByID(id);
        }
        [HttpPost("add-book")]
        public async Task<bool> addbook(Guid bookid, Guid? ManufacturerID, Guid? FormID, Guid? CouponID, string BookName, int TotalQuantity, string MainPhoto, int QuantitySold, int QuantityExists, decimal EntryPrice, decimal Price, string Information, string Description, string ISBN, int YearOfRelease, DateTime? DeleteDate, int TransactionStatus, int Status, int weight, decimal volume)
        {
            Book b = new Book();
            b.BookID = bookid;
            b.ManufacturerID = ManufacturerID;
            b.FormID = FormID;
            b.CouponID = CouponID;
            b.BookName = BookName;
            b.TotalQuantity = TotalQuantity;
            b.MainPhoto = MainPhoto;
            b.QuantitySold = QuantitySold;
            b.QuantityExists = QuantityExists;
            b.EntryPrice = EntryPrice;
            b.Price = Price;
            b.Information = Information;
            b.Description = Description;
            b.ISBN = ISBN;
            b.YearOfRelease = YearOfRelease;
            b.CreateDate = DateTime.Now;
            b.DeleteDate = DeleteDate;
            b.Weight = weight;
            b.Volume = volume;
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
        public async Task<bool> UpdateBook(Guid id, [FromBody] Book book)
        {
            var listBook = await ires.GetAll();
            var b = listBook.FirstOrDefault(c => c.BookID == id);
            if (b != null)
            {

                b.ManufacturerID = book.ManufacturerID;
                b.FormID = book.FormID;
                b.CouponID = book.CouponID;
                b.BookName = book.BookName;
              //  b.TotalQuantity = book.TotalQuantity;
                b.MainPhoto = book.MainPhoto;
            //    b.QuantitySold = book.QuantitySold;
                //b.QuantityExists = book.QuantityExists;
                //b.EntryPrice = book.EntryPrice;
                b.Information = book.Information;
                b.Description = book.Description;
                b.ISBN = book.ISBN;
                b.YearOfRelease = book.YearOfRelease;
                b.CreateDate = book.CreateDate;
                b.DeleteDate = book.DeleteDate;
                b.Weight = book.Weight;
                b.Volume = book.Volume;
                b.TransactionStatus = book.TransactionStatus;
                b.Status = book.Status;
                return await ires.UpdateItem(b);
            }
            else
            {
                return false;
            }
        }
        [HttpPost("[Action]")]
        public async Task<bool> CheckQuantity(Guid BookID, int Quantity)
        {
            var books = await ires.GetAll();
            var book = books.FirstOrDefault(p => p.BookID == BookID);
            if (book != null)
            {
                if (Quantity < book.QuantityExists)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else { return false; }
        }
        [HttpPut("UpdateQuantity")]
        public async Task<bool> UpdateQuantityBook(Guid id, int TotalQuantity, int QuantitySold, int QuantityExists)
        {
            var book = await ires.GetByID(id);
            if (book == null)
            {
                return false;
            }
            book.TotalQuantity = TotalQuantity;
            book.QuantitySold = QuantitySold;
            book.QuantityExists = QuantityExists;

            return await ires.UpdateItem(book);
        }
        [HttpPut("BuyBook")]
        public async Task<bool> BuyBook(Guid id, int quantityBuy)
        {
            var book = await ires.GetByID(id);
            if (book == null)
            {
                return false;
            }
            book.QuantitySold += quantityBuy;
            book.QuantityExists -= quantityBuy;

            return await ires.UpdateItem(book);
        }

        [HttpPost("PlusBook")]
        public async Task<bool> PlusBook(Guid id, int quantity)
        {
            var book = await ires.GetByID(id);
            if (book == null)
            {
                return false;
            }
            book.QuantitySold -= quantity;
            book.QuantityExists += quantity;

            return await ires.UpdateItem(book);
        }
        [HttpPut("UpdateStatus/{id}")]
        public async Task<IActionResult> UpdateStatus(Guid id, int newStatus)
        {
            try
            {
                var listBook = await ires.GetAll();
                var book = listBook.FirstOrDefault(c => c.BookID == id);

                if (book == null)
                {
                    return NotFound();
                }

                if (newStatus == 0 || newStatus == 1)
                {
                    book.Status = newStatus;
                    await ires.UpdateItem(book);

                    return Ok(new { status = true, message = "Cập nhật trạng thái thành công." });
                }
                else
                {
                    return BadRequest(new { status = false, message = "Giá trị trạng thái không hợp lệ." });
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = false, message = "Có lỗi xảy ra khi cập nhật trạng thái." });
            }
        }

    }
}

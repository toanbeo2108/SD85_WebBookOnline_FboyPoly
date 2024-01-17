using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;
namespace SD85_WebBookOnline.Api.Controllers
{
    [Route("api/Combo")]
    [ApiController]
    public class ComboController : ControllerBase
    {
        private readonly IAllResponsitories<Combo> irespon;
        private readonly IAllResponsitories<ComboItem> irespon_comboitem;
        private readonly IAllResponsitories<Book> irespon_book;
        AppDbContext context = new AppDbContext();
        public ComboController()
        {
            irespon = new AllResponsitories<Combo>(context, context.Combo);
            irespon_comboitem = new AllResponsitories<ComboItem>(context, context.ComboItem);
            irespon_book = new AllResponsitories<Book>(context, context.Book);
        }
        
        [HttpGet("[Action]")]
        public async Task<IEnumerable<Combo>> GetAllCombo()
        {
            return await irespon.GetAll();
        }
        [HttpPost("[Action]")]

        public async Task<bool> CreateCombo(Guid ComBoId,Guid createbyID, Guid cartItemID, string comboName,int quanTity, decimal price,string image /*IFormFile imageFile,*/)
        {
            Combo cb = new Combo();
            cb.ComboID = ComBoId;
            cb.CreatebyID = createbyID;
            cb.CartItemID = cartItemID;
            cb.ComboName = comboName;
            cb.Quantity = quanTity;
            cb.Price = price;
            cb.Status = 1;
            cb.Image = image;

            return await irespon.CreateItem(cb);

            // Xử lý tệp ảnh
            //if (imageFile != null && imageFile.Length > 0)
            //{
            //    // Lưu tệp ảnh vào thư mục trên máy chủ
            //    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            //    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            //    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            //    using (var fileStream = new FileStream(filePath, FileMode.Create))
            //    {
            //        await imageFile.CopyToAsync(fileStream);
            //    }

            //    // Cập nhật đường dẫn đến tệp ảnh trong thuộc tính Image của Combo
            //    cb.Image = "/images/" + uniqueFileName; // Đường dẫn dựa vào thư mục bạn lưu trữ tệp ảnh trong wwwroot
            //}
        }



        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateCombo(Guid id, [FromBody] Combo? dm)
        {
            var lstcb = await irespon.GetAll();
            var cb = lstcb.FirstOrDefault(x => x.ComboID == id);
            if (cb == null)
            {
                return false;
            }
            else
            {
                cb.CreatebyID = dm.CreatebyID;
                cb.CartItemID = dm.CartItemID;
                cb.ComboName = dm.ComboName;
                cb.Image = dm.Image;
                cb.Status = dm.Status;
                return await irespon.UpdateItem(cb);
            }

        }
        [HttpDelete("[Action]/{id}")]
        public async Task<bool> DeleteCombo(Guid id)
        {
            var lstcb = await irespon.GetAll();
            var cb = lstcb.FirstOrDefault(x => x.ComboID == id);
            if (cb == null)
            {
                return false;
            }
            else
            {
                return await irespon.DeleteItem(cb);
            }
        }
        [HttpPost("[Action]")]
        public async Task<bool> CheckQuantity(Guid ComboID, int Quantity)
        {
            var combos = await irespon.GetAll();
            var Combo = combos.FirstOrDefault(p => p.ComboID == ComboID);
            if (Combo != null)
            {
                if (Quantity < Combo.Quantity)
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

        [HttpPut("BuyCombo")]
        public async Task<bool> BuyCombo(Guid id, int quantityBuy)
        {
            var combo = await irespon.GetByID(id);
            var listcomboItem = await irespon_comboitem.GetAll();
            var comboitems = listcomboItem.Where(p => p.ComboID == id).ToList();
            var listbook = await irespon_book.GetAll();
            foreach( var comboItem in comboitems )
            {
                foreach (var book in listbook)
                {
                    if (comboItem.BookID == book.BookID)
                    {
                        book.QuantitySold += quantityBuy;
                        await irespon_book.UpdateItem(book);
                    }
                }
            }

            if (combo == null)
            {
                return false;
            }
            combo.Quantity -= quantityBuy;

            return await irespon.UpdateItem(combo);
        }
        [HttpPut("CancelBill")]
        public async Task<bool> CancelBill(Guid id, int quantityBuy)
        {
            var combo = await irespon.GetByID(id);
            var listcomboItem = await irespon_comboitem.GetAll();
            var comboitems = listcomboItem.Where(p => p.ComboID == id).ToList();
            var listbook = await irespon_book.GetAll();
            foreach (var comboItem in comboitems)
            {
                foreach (var book in listbook)
                {
                    if (comboItem.BookID == book.BookID)
                    {
                        book.QuantitySold -= quantityBuy;
                        await irespon_book.UpdateItem(book);
                    }
                }
            }

            if (combo == null)
            {
                return false;
            }
            combo.Quantity += quantityBuy;

            return await irespon.UpdateItem(combo);
        }
    }
}

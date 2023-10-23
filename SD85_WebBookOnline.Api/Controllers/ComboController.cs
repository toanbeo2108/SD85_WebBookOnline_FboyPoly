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
        AppDbContext context = new AppDbContext();
        public ComboController()
        {
            irespon = new AllResponsitories<Combo>(context, context.Combo);
        }
        [Authorize]
        [HttpGet("[Action]")]
        public async Task<IEnumerable<Combo>> GetAllCombo()
        {
            return await irespon.GetAll();
        }
        [HttpPost("[Action]")]
        public async Task<bool> CreateCombo(Guid createbyID, Guid cartItemID, string comboName, decimal price,string image, /*IFormFile imageFile,*/ int status)
        {
            Combo cb = new Combo();
            cb.ComboID = Guid.NewGuid();
            cb.CreatebyID = createbyID;
            cb.CartItemID = cartItemID;
            cb.ComboName = comboName;
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
    }
}

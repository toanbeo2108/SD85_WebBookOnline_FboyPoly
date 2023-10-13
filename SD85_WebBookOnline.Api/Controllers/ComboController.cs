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
        [HttpGet("[Action]")]
        public async Task<IEnumerable<Combo>> GetAllBill()
        {
            return await irespon.GetAll();
        }
        [HttpPost("[Action]")]
        public async Task<bool> CreateCombo(Guid createbyID, Guid cartItemID, string comboName, decimal price, string image, int status)
        {
            Combo cb = new Combo();
            cb.ComboID = Guid.NewGuid();
            cb.CreatebyID = createbyID;
            cb.CartItemID = cartItemID;
            cb.ComboName = comboName;
            cb.Price = price;
            cb.Image = image;
            cb.Status = 1;
            return await irespon.CreateItem(cb);
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

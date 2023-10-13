using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;

namespace SD85_WebBookOnline.Api.Controllers
{
    [Route("api/ComboItem")]
    [ApiController]
    public class ComboItemController : ControllerBase
    {
        private readonly IAllResponsitories<ComboItem> _irp;
        AppDbContext _context = new AppDbContext();
        public ComboItemController()
        {
            _irp = new AllResponsitories<ComboItem>(_context, _context.ComboItem);
        }
        [HttpGet("GetAll-ComboItem")]
        public async Task<IEnumerable<ComboItem>> Getall()
        {
            return await _irp.GetAll();
        }
        [HttpPost("Add-ComboItem")]
        public async Task<bool> createCBItem(Guid? BookID, Guid? ComboID, string ItemName, decimal Price, decimal Quantity, decimal ToTal, int Status)
        {
            ComboItem cbi = new ComboItem();
            cbi.ComboItemID = Guid.NewGuid();
            cbi.BookID = BookID;
            cbi.ComboID = ComboID;
            cbi.ItemName = ItemName;
            cbi.Price = Price;
            cbi.Quantity = Quantity;
            cbi.ToTal = ToTal;
            cbi.Status = Status;
            return await _irp.CreateItem(cbi);
        }
        [HttpPost("Update-ComboItem")]
        public async Task<bool> UpdateCBItem(Guid id , [FromBody]CartItems cartitem)
        {
            var list = await _irp.GetAll();
            var cbi = list.FirstOrDefault(c => c.ComboItemID == id);
            if (cbi!= null)
            {
                cbi.BookID = cartitem.BookID;
                cbi.ComboID = cartitem.ComboID;
                cbi.ItemName = cartitem.ItemName;
                cbi.Price = cartitem.Price;
                cbi.Quantity = Convert.ToDecimal(cartitem.Quantity);
                cbi.ToTal = cartitem.ToTal;
                cbi.Status = cartitem.Status;
                return await _irp.UpdateItem(cbi);
            }
            else
            {
                return false;
            }
           
        }
        [HttpDelete("Delete-ComboItem/{id}")]
        public async Task<bool> DeleteCBItem(Guid id)
        {
            var list = await _irp.GetAll();
            var cbi = list.FirstOrDefault(c => c.ComboItemID == id);
            if (cbi != null)
            {
               
                return await _irp.DeleteItem(cbi);
            }
            else
            {
                return false;
            }

        }
    }
}

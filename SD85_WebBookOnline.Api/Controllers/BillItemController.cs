using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;

namespace SD85_WebBookOnline.Api.Controllers
{
	[Route("api/BillItem")]
	[ApiController]
	public class BillItemController : ControllerBase
	{
		private readonly IAllResponsitories<BillItems> irespon;
		AppDbContext context = new AppDbContext();
        public BillItemController()
        {
            irespon = new AllResponsitories<BillItems>(context,context.BillItems);
        }
		[HttpGet("[Action]")]
		public async Task<IEnumerable<BillItems>> GetAllBillItem()
		{
			return await irespon.GetAll();	
		}
        [HttpGet("[Action]/{BillID}")]
        public async Task<IEnumerable<BillItems>> GetAllBillItemByBillID(Guid BillID)
        {
            return await context.BillItems.Where(p => p.BillID == BillID).ToListAsync();
        }
        [HttpPost("[Action]")]
		public async Task<bool> CreateBillItem(Guid? bookid, Guid? comboid, Guid? billid, string itemname,decimal price, int quantity,decimal total, decimal giaNhap)
		{
			var lstbillItem = await irespon.GetAll();
			BillItems bt = new BillItems();
			bt.BillItemID = Guid.NewGuid();
			bt.BillID = billid;
			bt.BookID = bookid;
			bt.ComboID = comboid;
			bt.ItemName = itemname;
			bt.Price = price;
			bt.GiaNhap = giaNhap;
			bt.Quantity = quantity;
			bt.ToTal = total;
			bt.Status = 1;
			//context.BillItems.Add(bt);
			//await context.SaveChangesAsync();

			return await irespon.CreateItem(bt);
		}
		[HttpPut("[Action]/{id}")]
		public async Task<bool> UpdateBillItem(Guid id, [FromBody] BillItems? dm)
		{
			var lstbt = await irespon.GetAll();
			var bt = lstbt.FirstOrDefault(x => x.BillItemID == id);
			if (bt == null)
			{
				return false;
			}
			else
			{
				bt.BillID = dm.BillID;
				bt.BookID = dm.BookID;
				bt.ComboID = dm.ComboID;
				bt.ItemName = dm.ItemName;
				bt.Price = dm.Price;
				bt.Quantity = dm.Quantity;
				bt.ToTal = dm.ToTal;
				bt.Status = dm.Status;
				return await irespon.UpdateItem(bt);
			}

		}
		[HttpDelete("[Action]/{id}")]
		public async Task<bool> DeleteBillItem(Guid id)
		{
			var lstbt = await irespon.GetAll();
			var bt = lstbt.FirstOrDefault(x => x.BillItemID == id);
			if (bt == null)
			{
				return false;
			}
			else
			{
				return await irespon.DeleteItem(bt);
			}

		}
	}
}

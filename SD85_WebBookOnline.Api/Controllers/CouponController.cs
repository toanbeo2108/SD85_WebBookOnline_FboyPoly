using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;

namespace SD85_WebBookOnline.Api.Controllers
{
	[Route("api/Coupon")]
	[ApiController]
	public class CouponController : ControllerBase
	{
		private readonly IAllResponsitories<Coupon> irespon;
		AppDbContext context = new AppDbContext();
        public CouponController()
        {
            irespon = new AllResponsitories<Coupon>(context,context.Coupon);
        }
		[HttpGet("[Action]")]
		public async Task<IEnumerable<Coupon>> GetAllCoupon()
		{
			return await irespon.GetAll();
		}
		[HttpPost("[Action]")]
		public async Task<bool> CreateCoupon(string couponame, int percentdiscount, DateTime startDate, DateTime enddate, string description)
		{

			var lstcp = await irespon.GetAll();
			var cp = lstcp.FirstOrDefault(x => x.CouponName == couponame);
			if(cp != null)
			{
				return false;
			}
			else
			{
				Coupon ncp = new Coupon();
				ncp.CouponID = Guid.NewGuid();
				ncp.CouponName = couponame;
				ncp.PercentDiscount = percentdiscount;
				ncp.StartDate = startDate;
				ncp.EndDate = enddate;
				ncp.Description = description;
				ncp.Status = 1;
				return await irespon.CreateItem(ncp);
			}
			
		}
		[HttpPut("[Action]/{id}")]
		public async Task<bool> UpdateCoupon(Guid id, [FromBody] Coupon? dm)
		{
			var lstbt = await irespon.GetAll();
			var bt = lstbt.FirstOrDefault(x => x.CouponID == id);
			if (bt == null)
			{
				return false;
			}
			else
			{

				bt.CouponName = dm.CouponName;
				bt.PercentDiscount = dm.PercentDiscount;
				bt.StartDate = dm.StartDate;
				bt.EndDate = dm.EndDate;
				bt.Description = dm.Description;
				bt.Status = dm.Status;
				return await irespon.UpdateItem(bt);
			}

		}
		[HttpDelete("[Action]/{id}")]
		public async Task<bool> DeleteCoupon(Guid id){ 
			var lstbt = await irespon.GetAll();
			var bt = lstbt.FirstOrDefault(x => x.CouponID == id);
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

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;

namespace SD85_WebBookOnline.Api.Controllers
{
	[Route("api/Languge")]
	[ApiController]
	public class LangugeController : ControllerBase
	{
		private readonly IAllResponsitories<Languge> irespon;
		AppDbContext context = new AppDbContext();
        public LangugeController()
        {
            irespon = new AllResponsitories<Languge>(context, context.Languges);
        }
		[HttpGet("[Action]")]
		public async Task<IEnumerable<Languge>> GetAllLanguge()
		{
			return await irespon.GetAll();
		}
		[HttpPost("[Action]")]
		public async Task<bool> CreateLanguge(string name, string? description,int status)
		{

			var lstcp = await irespon.GetAll();
			var cp = lstcp.FirstOrDefault(x => x.Name == name);
			if (cp != null)
			{
				return false;
			}
			else
			{
				Languge lg = new Languge();
				lg.LangugeID = Guid.NewGuid();
				lg.Name = name;
				lg.Description = description;
				lg.Status = 1;
				return await irespon.CreateItem(lg);

			}
		}
		[HttpPut("[Action]/{id}")]
		public async Task<bool> UpdateLaguge(Guid id, [FromBody] Languge? dm)
		{
			var lstbt = await irespon.GetAll();
			var bt = lstbt.FirstOrDefault(x => x.LangugeID == id);
			if (bt == null)
			{
				return false;
			}
			else
			{

				bt.Name = dm.Name;
				bt.Description = dm.Description;
				bt.Status = dm.Status;
				return await irespon.UpdateItem(bt);
			}

		}
		[HttpDelete("[Action]/{id}")]
		public async Task<bool> DeleteLaguge(Guid id)
		{
			var lstbt = await irespon.GetAll();
			var bt = lstbt.FirstOrDefault(x => x.LangugeID == id);
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

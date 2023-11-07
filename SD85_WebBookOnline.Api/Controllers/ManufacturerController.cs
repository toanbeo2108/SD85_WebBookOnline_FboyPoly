using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;

namespace SD85_WebBookOnline.Api.Controllers
{
	[Route("api/Manufacturer")]
	[ApiController]
	public class ManufacturerController : ControllerBase
	{
		private readonly IAllResponsitories<Manufacturer> irespon;
		AppDbContext context = new AppDbContext();
        public ManufacturerController()
        {
            irespon = new AllResponsitories<Manufacturer>(context, context.Manufacturer);

        }
		[HttpGet("[Action]")]
		public async Task<IEnumerable<Manufacturer>> GetAllManufacturer()
		{
			return await irespon.GetAll();
		}
		[HttpPost("[Action]")]
		public async Task<bool> CreateManufacture(string name, string? description, int status)
		{

			var lstcp = await irespon.GetAll();
			var cp = lstcp.FirstOrDefault(x => x.ManufactureName == name);
			if (cp != null)
			{
				return false;
			}
			else
			{
				Manufacturer lg = new Manufacturer();
				lg.ManufactureID = Guid.NewGuid();
				lg.ManufactureName = name;
				lg.Desciption = description;
				lg.Status = 1;
				return await irespon.CreateItem(lg);

			}
		}
		[HttpPut("[Action]/{id}")]
		public async Task<bool> UpdateManufacturer(Guid id, [FromBody] Manufacturer? dm)
		{
			var lstbt = await irespon.GetAll();
			var bt = lstbt.FirstOrDefault(x => x.ManufactureID == id);
			if (bt == null)
			{
				return false;
			}
			else
			{

				bt.ManufactureName = dm.ManufactureName;
				bt.Desciption = dm.Desciption;
				bt.Status = dm.Status;
				return await irespon.UpdateItem(bt);
			}

		}
		[HttpDelete("[Action]/{id}")]
		public async Task<bool> DeleteManufacturer(Guid id)
		{
			var lstbt = await irespon.GetAll();
			var bt = lstbt.FirstOrDefault(x => x.ManufactureID == id);
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

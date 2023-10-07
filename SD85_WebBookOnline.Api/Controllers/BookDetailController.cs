using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;

namespace SD85_WebBookOnline.Api.Controllers
{
	[Route("api/BookDetail")]
	[ApiController]
	public class BookDetailController : ControllerBase
	{
		private readonly IAllResponsitories<BookDetail> irespon;
		AppDbContext context = new AppDbContext();
        public BookDetailController()
        {
            irespon = new AllResponsitories<BookDetail>(context,context.BookDetails);

        }
		[HttpGet("[Action]")]
		public async Task<IEnumerable<BookDetail>> GetAllBookDetail()
		{
			return await irespon.GetAll();
		}
		[HttpPost("[Action]")]
		public async Task<bool> CreateBookDetail(Guid? bookid, Guid? categoryid, Guid? authorid,Guid? langugeid)
		{
			
			BookDetail bd = new BookDetail();
			bd.BookDetailID = Guid.NewGuid();	
			bd.BookID = bookid;
			bd.CategoriesID = categoryid;
			bd.AuthorID = authorid;
			bd.LagugeID = langugeid;
			return await irespon.CreateItem(bd);
		}
		[HttpPut("[Action]/{id}")]
		public async Task<bool> UpdateBookDetail(Guid id, [FromBody] BookDetail? dm)
		{
			var lstbt = await irespon.GetAll();
			var bt = lstbt.FirstOrDefault(x => x.BookDetailID == id);
			if (bt == null)
			{
				return false;
			}
			else
			{
				
				bt.BookID = dm.BookID;
				bt.CategoriesID = dm.CategoriesID;
				bt.Author = dm.Author;
				bt.LagugeID = dm.LagugeID;
				return await irespon.UpdateItem(bt);
			}

		}
		[HttpDelete("[Action]/{id}")]
		public async Task<bool> DeleteBookDetail(Guid id)
		{
			var lstbt = await irespon.GetAll();
			var bt = lstbt.FirstOrDefault(x => x.BookDetailID == id);
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

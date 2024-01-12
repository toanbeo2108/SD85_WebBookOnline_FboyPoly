using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;
namespace SD85_WebBookOnline.Api.Controllers
{
    [Route("api/Category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IAllResponsitories<Category> irespon;
        AppDbContext context = new AppDbContext();
        public CategoryController()
        {
            irespon = new AllResponsitories<Category>(context, context.Categories);
        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<Category>> GetAllCategory()
        {
            return await irespon.GetAll();
        }
        [HttpPost("[Action]")]
        public async Task<bool> CreateCategory(Guid CategoryParentID,string name, string description, int status)
        {
            Category c = new Category();
            c.CategoryID = Guid.NewGuid();
            c.CategoryParentID = CategoryParentID;
            c.Name = name;
            c.Description = description;
            c.Status = 1;
            return await irespon.CreateItem(c);
        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateCategory(Guid id, [FromBody] Category? dm)
        {
            var lstc = await irespon.GetAll();
            var c = lstc.FirstOrDefault(x => x.CategoryID == id);
            if (c == null)
            {
                return false;
            }
            else
            {
                c.CategoryParentID = dm.CategoryParentID;
                c.Name = dm.Name;
                c.Description = dm.Description;
                c.Status = dm.Status;
                return await irespon.UpdateItem(c);
            }

        }
        [HttpDelete("[Action]/{id}")]
        public async Task<bool> DeleteCategory(Guid id)
        {
            var lstc = await irespon.GetAll();
            var c = lstc.FirstOrDefault(x => x.CategoryID == id);
            if (c == null)
            {
                return false;
            }
            else
            {
                return await irespon.DeleteItem(c);
            }

        }
    }
}

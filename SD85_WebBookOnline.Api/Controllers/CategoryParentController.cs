using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;

namespace SD85_WebBookOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryParentController : ControllerBase
    {
        private readonly IAllResponsitories<CategoryParent> _irespon;
        AppDbContext _context = new AppDbContext();
        public CategoryParentController()
        {
            _irespon = new AllResponsitories<CategoryParent>(_context,_context.CategoryParents);
        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<CategoryParent>> GetAllCategoryParents()
        {
            return await _irespon.GetAll();
        }
        [HttpPost("[Action]")]
        public async Task<bool> CreateCategoryParent(Guid? caTegoryId, string? CategoryParentName)
        {
            CategoryParent ctd = new CategoryParent();
            ctd.CategoryParentID = Guid.NewGuid();
            ctd.CategoryParentName = CategoryParentName;
            ctd.Status = 1;
            return await  _irespon.CreateItem(ctd);
        }
        [HttpGet("[Action]/{id}")]
        public async Task<bool> UpdateCategory(Guid id, [FromBody] CategoryParent? cTd)
        {
            var lstctd = await _irespon.GetAll();
            var ctd = lstctd.FirstOrDefault(x =>x.CategoryParentID == id);
            if(ctd == null)
            {
                return false;
            }
            else
            {
                ctd.CategoryParentName = cTd.CategoryParentName;
                ctd.Status = cTd.Status;
            }
            return await _irespon.UpdateItem(ctd);
        }
        [HttpDelete("[Action]/{id}")]
        public async Task<bool> DeleteCategoryParent(Guid id)
        {
            var lstctd = await _irespon.GetAll();
            var ctd = lstctd.FirstOrDefault(x => x.CategoryParentID == id);
            if (ctd == null)
            {
                return false;
            }
            else
            {
                return await _irespon.DeleteItem(ctd);
            }
        }
    }
}

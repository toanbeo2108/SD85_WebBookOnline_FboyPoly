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
    public class PostbannerController : ControllerBase
    {
        private readonly IAllResponsitories<PostBanner> _irp;
        AppDbContext _context = new AppDbContext();
        public PostbannerController()
        {
            _irp = new AllResponsitories<PostBanner>(_context, _context.PostBanner);
        }
        [HttpGet("GetAll-PostBanner")]
        public async Task<IEnumerable<PostBanner>> GetAll()
        {
            return   await _irp.GetAll();   
        }
        [HttpPost("Create-postBaner")]
        public async Task<bool> AddPostBanner(string Images, DateTime PostDate, string Title, string Content, string Status) 
        {
            PostBanner p = new PostBanner();
            p.PostID = Guid.NewGuid();
            p.Images = Images;
            p.PostDate = PostDate;
            p.Title = Title;
            p.Content = Content;
            p.Status = Status;

            return await _irp.CreateItem(p);
        }
        [HttpPut("Update-postBaner")]
        public async Task<bool> UpdatePostBanner(Guid id, [FromBody] PostBanner pb) 
        {
            var list = await _irp.GetAll();
            var p = list.FirstOrDefault(c => c.PostID == id);
            if (p != null)
            {
                p.Images = pb.Images;
                p.PostDate = pb.PostDate;
                p.Title = pb.Title;
                p.Content = pb.Content;
                p.Status = pb.Status;
                return await _irp.UpdateItem(p);
            }

            else
            {
                return false;
            }
            
        }
        [HttpDelete("Delete-postBaner")]
        public async Task<bool> DeletePostBanner(Guid id)
        {
            var list = await _irp.GetAll();
            var p = list.FirstOrDefault(c => c.PostID == id);
            if (p != null)
            {
                return await _irp.DeleteItem(p);
            }

            else
            {
                return false;
            }

        }
    }
}

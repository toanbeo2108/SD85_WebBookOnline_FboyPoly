using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;

namespace SD85_WebBookOnline.Api.Controllers
{
    [Route("api/Rating")]
    [ApiController]
    public class RatingController : Controller
    {
        private readonly IAllResponsitories<Rating> irespon;
        AppDbContext context = new AppDbContext();
        public RatingController()
        {
            irespon = new AllResponsitories<Rating>(context, context.Ratings);
        }

        [HttpGet("[Action]")]
        public async Task<IEnumerable<Rating>> GetAllRating()
        {
            return await irespon.GetAll();
        }
        [HttpPost("[Action]")]
        public async Task<bool> CreateRating(Guid? idbook,string? iduser,string?comment,int? stars)
        {
            Rating r = new Rating();
            r.ID = Guid.NewGuid();
            r.IdBook = idbook;
            r.IdNguoiDung = iduser;
            r.Comment = comment;
            r.Stars = stars;
            r.RatingDate = DateTime.Now;
            r.Status = 0;
            return await irespon.CreateItem(r);
        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateRating(Guid id, [FromBody] Rating? rt) 
        {
            var lstrt = await irespon.GetAll();
            var r = lstrt.FirstOrDefault(x => x.ID == id);
            if (r == null)
            {
                return false;
            }
            else
            {
                rt.IdNguoiDung = r.IdNguoiDung;
                rt.IdBook = rt.IdBook;
                r.RatingDate = DateTime.Now;
                r.Comment = rt.Comment;
                r.Stars = rt.Stars;
                r.Status = rt.Status;
                return await irespon.UpdateItem(r);
            }

        }
        [HttpDelete("[Action]/{id}")]
        public async Task<bool> DeleteRating(Guid id)
        {
            var lstv = await irespon.GetAll();
            var v = lstv.FirstOrDefault(x => x.ID == id);
            if (v == null)
            {
                return false;
            }
            else
            {
                return await irespon.DeleteItem(v);
            }
        }

    }
    
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;


namespace SD85_WebBookOnline.Api.Controllers
{
    [Route("api/Image")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IAllResponsitories<Images> _irp;
        AppDbContext context = new AppDbContext();
        public ImageController()
        {
            _irp = new AllResponsitories<Images>(context, context.Images);
        }
        [HttpGet("getAll_Image")]
        public async Task<IEnumerable<Images>> gettAll()
        {
           
            return await _irp.GetAll();
        }
        [HttpPost("add-image")]
        public async Task<bool> addImage(Guid? BookID, string ImageName, int Status)
        {
            Images img = new Images();
            img.ImagesID = Guid.NewGuid();
            img.BookID = BookID;
            img.Status = Status;
            img.ImageName = ImageName;
            return await _irp.CreateItem(img);              
        }
        [HttpDelete("delete-image/{id}")]
        public async Task<bool> deleteImage(Guid id)
        {
            var list = await _irp.GetAll();
            var img = list.FirstOrDefault(c=>c.ImagesID==id);
            if (img != null)
            {
                return await _irp.DeleteItem(img);
            }
            else
            {
                return false;
            }
        }
        [HttpPut("Update-image/{id}")]
        public async Task<bool> UpdateImage(Guid id, [FromBody] Images images)
        {
            var list = await _irp.GetAll();
            var img = list.FirstOrDefault(c=>c.ImagesID==id);
            if (img != null)
            {
                
                img.BookID = images.BookID;
                img.Status = images.Status;
                img.ImageName = images.ImageName;
                return await _irp.UpdateItem(img);
               
            }
            else
            {
                return false;
            }
        }
    }
}

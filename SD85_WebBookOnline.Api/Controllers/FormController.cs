using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;
namespace SD85_WebBookOnline.Api.Controllers
{
    [Route("api/Form")]
    [ApiController]
    public class FormController
    {
        private readonly IAllResponsitories<Form> irespon;
        AppDbContext context = new AppDbContext();
        public FormController()
        {
            irespon = new AllResponsitories<Form>(context, context.Form);
        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<Form>> GetAllForm()
        {
            return await irespon.GetAll();
        }
        [HttpPost("[Action]")]
        public async Task<bool> CreateForm(string formName, string description, int status)
        {
            Form f = new Form();
            f.FormId = Guid.NewGuid();
            f.FormName = formName;
            f.Description = description;
            f.Status = 1;
            return await irespon.CreateItem(f);
        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateForm(Guid id, [FromBody] Form? dm)
        {
            var lstf = await irespon.GetAll();
            var f = lstf.FirstOrDefault(x => x.FormId == id);
            if (f == null)
            {
                return false;
            }
            else
            {
                f.FormName = dm.FormName;
                f.Description = dm.Description;
                f.Status = dm.Status;
                return await irespon.UpdateItem(f);
            }

        }
        [HttpDelete("[Action]/{id}")]
        public async Task<bool> DeleteForm(Guid id)
        {
            var lstf = await irespon.GetAll();
            var f = lstf.FirstOrDefault(x => x.FormId == id);
            if (f == null)
            {
                return false;
            }
            else
            {
                return await irespon.DeleteItem(f);
            }
        }
    }
}

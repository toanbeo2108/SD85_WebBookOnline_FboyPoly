using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;

namespace SD85_WebBookOnline.Api.Controllers
{
    [Route("api/InputSlipController")]
    [ApiController]
    public class InputSlipController : ControllerBase
    {
        private readonly IAllResponsitories<InputSlip> _irespon;
        AppDbContext _context = new AppDbContext();
        public InputSlipController()
        {
            _context = new AppDbContext();
            _irespon = new AllResponsitories<InputSlip>(_context, _context.InputSlip);
        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<InputSlip>> GetAllInputSlip()
        {
            return await _irespon.GetAll();
        }
        [HttpPost("[Action]")]
        public async Task<bool> CreateInputSlip(Guid? idNhanVienNhap, Guid? idSachNhap, int soLuong, DateTime ngayNhap)
        {
            //var lstauthor = await _irespon.GetAll();
            //var insp = lstauthor.FirstOrDefault(x => x.AuthorName == authorName);
            //if (insp != null)
            //{
            //    return false;
            //}
            InputSlip insp = new InputSlip();
            insp.InputSlipID = new Guid();
            insp.IdNhanVienNhap = idNhanVienNhap;
            insp.IdSachNhap = idSachNhap;
            insp.SoLuong = soLuong;
            insp.NgayNhap = ngayNhap;

            return await _irespon.CreateItem(insp);
        }

        [HttpDelete("[Action]/{id}")]
        public async Task<bool> UpdateInputSlip(Guid id, Guid? idNhanVienNhap, Guid? idSachNhap, int soLuong, DateTime ngayNhap)
        {
            var lstauthor = await _irespon.GetAll();
            var insp = lstauthor.FirstOrDefault(x => x.InputSlipID == id);
            if (insp != null)
            {
                return false;
            }
            insp.IdNhanVienNhap = idNhanVienNhap;
            insp.IdSachNhap = idSachNhap;
            insp.SoLuong = soLuong;
            insp.NgayNhap = ngayNhap;

            return await _irespon.CreateItem(insp);
        }
    }
}


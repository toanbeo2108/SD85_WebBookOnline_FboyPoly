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
            _irespon = new AllResponsitories<InputSlip>(_context, _context.InputSlips);
        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<InputSlip>> GetAllInputSlip()
        {
            return await _irespon.GetAll();
        }
        [HttpPost("[Action]")]
        public async Task<bool> CreateInputSlip(string? idNhanVienNhap, Guid? idSachNhap, decimal giaban, int soLuong, DateTime ngayNhap, decimal giaNhap)
        {
            InputSlip insp = new InputSlip();
            var book = await _context.Book.FindAsync(idSachNhap);
            if (book == null)
            {
                return false;

            }
            else
            {

                insp.InputSlipID = Guid.NewGuid();
                insp.IdNhanVienNhap = idNhanVienNhap;
                insp.IdSachNhap = idSachNhap;
                insp.SoLuong = soLuong;
                insp.NgayNhap = ngayNhap;
                insp.GiaNhap = giaNhap;
                insp.GiaBan = giaban;
                book.QuantityExists += soLuong ;
                book.TotalQuantity += soLuong ;
                book.Price = giaban;
                book.EntryPrice = giaNhap;
                return await _irespon.CreateItem(insp);
            }
        }

        [HttpDelete("[Action]/{id}")]
        public async Task<bool> UpdateInputSlip(Guid id, string? idNhanVienNhap, Guid? idSachNhap, int soLuong, DateTime ngayNhap)
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


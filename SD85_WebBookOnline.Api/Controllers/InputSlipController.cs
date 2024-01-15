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
        public async Task<bool> CreateInputSlip(Guid? idNhanVienNhap, Guid? idSachNhap,decimal giaban, int soLuong, DateTime ngayNhap,decimal giaNhap)
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
                book.QuantityExists += insp.SoLuong  ?? 0;
                book.TotalQuantity += insp.SoLuong  ?? 0;
                book.Price = giaban;
                book.EntryPrice = giaNhap;
                return await _irespon.CreateItem(insp);
            }
        }

        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateInputSlip(Guid id, Guid? idNhanVienNhap, Guid? idSachNhap, int soLuong, DateTime ngayNhap, decimal giaNhap)
        {
            var lstauthor = await _irespon.GetAll();
            var insp = lstauthor.FirstOrDefault(x => x.InputSlipID == id);
            if (insp == null)
            {
                return false;
            }
          //  insp.IdNhanVienNhap = idNhanVienNhap;
            insp.IdSachNhap = idSachNhap;
            insp.SoLuong = soLuong;
            insp.NgayNhap = ngayNhap;
            insp.GiaNhap = giaNhap;

            return await _irespon.CreateItem(insp);
        }
        [HttpPut("Update-image/{id}")]
        public async Task<bool> UpdateImage(Guid id, [FromBody] InputSlip input)
        {
            var list = await _irespon.GetAll();
            var book = await _context.Book.FindAsync(input.IdSachNhap);
            if (book != null)
            {

                var img = list.FirstOrDefault(c => c.InputSlipID == id);
                if (img != null)
                {

                    img.IdSachNhap = input.IdSachNhap;
                    img.SoLuong = input.SoLuong;
                    img.NgayNhap = input.NgayNhap;
                    img.GiaNhap = input.GiaNhap;
                    img.GiaBan = input.GiaBan;

                    book.QuantityExists += input.SoLuong ?? 0;
                    book.TotalQuantity += input.SoLuong ?? 0;
                    book.Price = (decimal)input.GiaBan;
                    return await _irespon.UpdateItem(img);

                }

                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}


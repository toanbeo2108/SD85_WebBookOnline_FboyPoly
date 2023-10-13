using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;

namespace SD85_WebBookOnline.Api.Controllers
{
    [Route("api/Cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IAllResponsitories<Cart> _irespon;
        AppDbContext _context = new AppDbContext();
        public CartController()
        {
            _irespon = new AllResponsitories<Cart>(_context,_context.Cart);
        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<Cart>> GetAllCart()
        {
            return await _irespon.GetAll();
        }
        [HttpPost("[Action]")]
        public async Task<bool> CreateCart(Guid? voucherID, decimal priceBeforeVoucher, decimal total)
        {
            
            Cart cr = new Cart();
            cr.CartId = Guid.NewGuid();
            cr.VoucherID = voucherID;
            cr.PriceBeforeVoucher = priceBeforeVoucher;
            cr.Total = total;
            cr.Status = 1;

            return await _irespon.CreateItem(cr);
        }

        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateCart(Guid cartId, Guid? voucherID, decimal priceBeforeVoucher, decimal total, int status)
        {
            var lstauthor = await _irespon.GetAll();
            var cr = lstauthor.FirstOrDefault(x => x.CartId == cartId);
            if (cr == null)
            {
                return false;
            }
            cr.VoucherID = voucherID;
            cr.PriceBeforeVoucher = priceBeforeVoucher;
            cr.Total = total;
            cr.Status = status;

            return await _irespon.UpdateItem(cr);
        }
        [HttpDelete("[Action]/{id}")]
        public async Task<bool> DeleteCart(Guid cartId, int status)
        {
            var lstauthor = await _irespon.GetAll();
            var cr = lstauthor.FirstOrDefault(x => x.CartId == cartId);
            if (cr == null)
            {
                return false;
            }

            cr.Status = 0;

            return await _irespon.UpdateItem(cr);
        }
    }
}

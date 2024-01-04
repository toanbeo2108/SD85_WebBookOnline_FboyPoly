using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Api.Responsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;

namespace SD85_WebBookOnline.Api.Controllers
{
    [Route("api/Cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartServices _cartReponsitories;
        public CartController(ICartServices reponsitories)
        {
            _cartReponsitories = reponsitories;
        }

        [HttpGet("[Action]")]
        public async Task<IEnumerable<Cart>> GetAllCart()
        {
            return await _cartReponsitories.GetAllCart();
        }
        [HttpGet("[Action]/{UserID}")]
        public async Task<List<Cart>> GetCartByIdUser(string UserID,int? status)
        {
            return await _cartReponsitories.GetListCartByUserID(UserID,status);
        }
        [HttpPost("[Action]")]
        public async Task<bool> CreateCart(Guid CartId, Guid? voucherID, string? UserId, decimal priceBeforeVoucher, decimal total)
        {
            return await _cartReponsitories.CreateCart(CartId, voucherID, UserId, priceBeforeVoucher, total);
        }
        [HttpPut("[Action]")]
        public async Task<bool> UpdateCart(Guid cartId, Guid? voucherID, decimal priceBeforeVoucher, decimal total, int status)
        {
            return await _cartReponsitories.UpdateCart(cartId, voucherID, priceBeforeVoucher, total, status);
        }

        [HttpDelete("[Action]/{id}")]
        public async Task<bool> DeleteCart(Guid id)
        {
            return await _cartReponsitories.DeleteCart(id);
        }
        
        // Code cũ, tạm thời chưa xóa do sợ dính bug =)))

        //-----------------------------------------------------------------------------------------
        //private readonly IAllResponsitories<Cart> _irespon;
        //private readonly AppDbContext _context;
        //public CartController()
        //{
        //    _irespon = new AllResponsitories<Cart>(_context, _context.Cart);
        //}

        //[HttpGet("[Action]")]
        //public async Task<IEnumerable<Cart>> GetAllCart()
        //{
        //    return await _irespon.GetAll();
        //}
        //[HttpPost("[Action]")]
        //public async Task<bool> CreateCart(Guid CartId, Guid? voucherID, string? UserId, decimal priceBeforeVoucher, decimal total)
        //{

        //    Cart cr = new Cart();
        //    cr.CartId = CartId;
        //    cr.VoucherID = voucherID;
        //    cr.UserID = UserId;
        //    cr.PriceBeforeVoucher = priceBeforeVoucher;
        //    cr.Total = total;
        //    cr.Status = 1;

        //    return await _irespon.CreateItem(cr);
        //}

        //[HttpPut("[Action]/{id}")]
        //public async Task<bool> UpdateCart(Guid cartId, Guid? voucherID, decimal priceBeforeVoucher, decimal total, int status)
        //{
        //    var lstauthor = await _irespon.GetAll();
        //    var cr = lstauthor.FirstOrDefault(x => x.CartId == cartId);
        //    if (cr == null)
        //    {
        //        return false;
        //    }
        //    cr.VoucherID = voucherID;
        //    cr.PriceBeforeVoucher = priceBeforeVoucher;
        //    cr.Total = total;
        //    cr.Status = status;

        //    return await _irespon.UpdateItem(cr);
        //}
        //[HttpDelete("[Action]/{id}")]
        //public async Task<bool> DeleteCart(Guid cartId, int status)
        //{
        //    var lstauthor = await _irespon.GetAll();
        //    var cr = lstauthor.FirstOrDefault(x => x.CartId == cartId);
        //    if (cr == null)
        //    {
        //        return false;
        //    }

        //    cr.Status = 0;

        //    return await _irespon.UpdateItem(cr);
        //}
    }
}

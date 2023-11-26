using Microsoft.EntityFrameworkCore;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Share.Models;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace SD85_WebBookOnline.Api.Responsitories
{
    public class CartServies : ICartServices
    {
        public AppDbContext _context {  get; set; }
        public CartServies(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateCart(Guid CartId, Guid? voucherID, string? UserId, decimal priceBeforeVoucher, decimal total)
        {
            try
            {
                Cart cr = new Cart();
                cr.CartId = CartId;
                cr.VoucherID = voucherID;
                cr.UserID = UserId;
                cr.PriceBeforeVoucher = priceBeforeVoucher;
                cr.Total = total;
                cr.Status = 1;

                _context.Cart.Add(cr);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
           
        }

        public async Task<bool> DeleteCart(Guid id)
        {
            try
            {
                var Cart = _context.Cart.FirstOrDefault(p => p.CartId == id);
                _context.Cart.Remove(Cart);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Cart>> GetAllCart()
        {
            return await _context.Cart.ToListAsync();
        }

        public async Task<List<Cart>> GetListCartByID(string id)
        {
            return await _context.Cart.Where(p => p.UserID == id).ToListAsync();
        }

        public async Task<bool> UpdateCart(Guid cartId, Guid? voucherID, decimal priceBeforeVoucher, decimal total, int status)
        {
            try
            {
                // Lấy cart cũ :
                Cart cart = _context.Cart.FirstOrDefault(c => c.CartId == cartId);
                // thay đổi thuộc tính của giỏ hàng cũ :
                cart.VoucherID = voucherID;
                cart.PriceBeforeVoucher = priceBeforeVoucher;
                cart.Total = total; 
                cart.Status = status;

                // xong r thì cập nhật và lưu vào db
                _context.Cart.Update(cart);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Cart>> GetListCartByUserID(string UserId, int? status)
        {
            if (status == null)
            {
                return await _context.Cart.Where(p => p.UserID == UserId).ToListAsync();
            }
            else
            {
                return await _context.Cart.Where(p => p.UserID == UserId && p.Status == status).ToListAsync();
            }
        }
    }
}

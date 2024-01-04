using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;

namespace SD85_WebBookOnline.Api.Controllers
{
    [Route("api/CartItem")]
    [ApiController]
    public class CartitemController : ControllerBase
    {
        private readonly IAllResponsitories<CartItems> _irp;
        private readonly IAllResponsitories<Book> _bookReponse;
        private readonly IAllResponsitories<Combo> _comboReponse;
        AppDbContext _context = new AppDbContext();
        public CartitemController()
        {
            _irp = new AllResponsitories<CartItems>(_context, _context.CartItems);
            _bookReponse = new AllResponsitories<Book>(_context, _context.Book);
            _comboReponse = new AllResponsitories<Combo>(_context, _context.Combo);
        }
        [HttpGet("GetAll-CartItem")]
        public async Task<IEnumerable<CartItems>> GetAllCartItem()
        {
            return await _irp.GetAll();
        }
        [HttpGet("[Action]/{CartID}")]
        public async Task<List<CartItems>> GetCartItemByCartID(Guid CartID)
        {
            return await _context.CartItems.Where(p => p.CartID == CartID).ToListAsync();
        }
        [HttpPost("Add-CartItem")]
        public async Task<bool> AddCartItem(Guid? CartID, Guid? ComboID, Guid? BookID,string? image, string ItemName, decimal Price, int Quantity, decimal ToTal, int Status)
        {
            CartItems c = new CartItems();
            c.CartItemID = Guid.NewGuid();
            c.CartID = CartID;
            c.ComboID = ComboID;
            c.BookID = BookID;
            c.ItemName = ItemName;
            c.Image = image;
            c.Price = Price;
            c.Quantity = Quantity;
            c.ToTal = ToTal;
            c.Status = Status;
            return await _irp.CreateItem(c);

        }
        [HttpPut("Update-CartItem/{id}")]
        public async Task<bool> updateCartItem(Guid id, [FromBody]CartItems cart)
        {
            var list = await _irp.GetAll();
            var c = list.FirstOrDefault(c=>c.CartItemID==id);
            if (c!= null)
            {
                
                c.CartID = cart.CartID;
                c.ComboID = cart.ComboID;
                c.BookID = cart.BookID;
                c.ItemName = cart.ItemName;
                c.Image = cart.Image;
                c.Price = cart.Price;
                c.Quantity = cart.Quantity;
                c.ToTal = cart.ToTal;
                c.Status = cart.Status;
                return await _irp.UpdateItem(c);
            }
            else
            {
                return false;
            }
        }
        [HttpDelete("Delete-CartItem/{id}")]
        public async Task<bool> DeletecartItem(Guid id)
        {
            var list = await _irp.GetAll();
            var c = list.FirstOrDefault(c => c.CartItemID == id);
            if (c != null)
            {

                return await _irp.DeleteItem(c);
            }
            else
            {
                return false;
            }
        }
        [HttpPost("[Action]")]
        public async Task<bool> CheckQuantity(Guid IdCartItems, int QuantityCartItem)
        {
            var list = await _irp.GetAll();
            var cartItem = list.FirstOrDefault(c => c.CartItemID == IdCartItems);
            var books = await _bookReponse.GetAll();
            var book = books.FirstOrDefault(p => p.BookID == cartItem.BookID);
            if (cartItem != null)
            {
                if (QuantityCartItem < book.QuantityExists) 
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else { return false; }
        }
    }
}

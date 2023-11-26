using SD85_WebBookOnline.Share.Models;

namespace SD85_WebBookOnline.Api.IResponsitories
{
    public interface ICartServices
    {
        Task<IEnumerable<Cart>> GetAllCart();
        Task<List<Cart>> GetListCartByUserID(string UserId,int? status);
        Task<bool> CreateCart(Guid CartId, Guid? voucherID, string? UserId, decimal priceBeforeVoucher, decimal total);
        Task<bool> DeleteCart(Guid id);
        Task<bool> UpdateCart(Guid CartId, Guid? voucherID, decimal priceBeforeVoucher, decimal total, int status);
    }
}

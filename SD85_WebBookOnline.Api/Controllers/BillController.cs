using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;
namespace SD85_WebBookOnline.Api.Controllers
{
    [Route("api/Bill")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IAllResponsitories<Bill> irespon;
        AppDbContext context = new AppDbContext();
        public BillController()
        {
            irespon = new AllResponsitories<Bill>(context, context.Bill);
        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<Bill>> GetAllBill()
        {
            return await irespon.GetAll();
        }
        [HttpGet("[Action]")]
        public async Task<List<Bill>> GetAllBillByUserID(string UserID)
        {
            return await context.Bill.Where(p => p.UserID == UserID).ToListAsync();
        }
        [HttpGet("[Action]/{id}")]
        public async Task<Bill> GetBillByBillId(Guid id)
        {
            return await context.Bill.FirstOrDefaultAsync(p => p.BillID == id);
        }
        [HttpPost("[Action]")]
        public async Task<bool> CreateBill(Guid? voucherID,string UserID, decimal priceBeforeVoucher, decimal shipmoney, string userPhone, string addressUser, DateTime orderDate, DateTime deliveryDate, decimal total, int paymentMethod, int status)
        {
            Bill b = new Bill();
            b.BillID = Guid.NewGuid();
            b.VoucherID = voucherID;
            b.UserID = UserID;
            b.PriceBeforeVoucher = priceBeforeVoucher;
            b.Shipmoney = shipmoney;
            b.UserPhone = userPhone;
            b.AddressUser = addressUser;
            b.OrderDate = orderDate;
            b.DeliveryDate = deliveryDate;
            b.Total = total;
            b.PaymentMethod = paymentMethod;
            b.Status = 1;
            return await irespon.CreateItem(b);
        }
        [HttpPost("[Action]")]
        public async Task<bool> CreateBillWithManualBillId(Guid BillID,Guid? voucherID,string ReceiverName,string Email, string UserID, decimal priceBeforeVoucher, decimal shipmoney, string userPhone, string addressUser, DateTime orderDate, DateTime deliveryDate, decimal total, int paymentMethod, int status)
        {
            Bill b = new Bill();
            b.BillID = BillID;
            b.VoucherID = voucherID;
            b.UserID = UserID;
            b.ReceiverName = ReceiverName;
            b.Email = Email;
            b.PriceBeforeVoucher = priceBeforeVoucher;
            b.Shipmoney = shipmoney;
            b.UserPhone = userPhone;
            b.AddressUser = addressUser;
            b.OrderDate = orderDate;
            b.DeliveryDate = deliveryDate;
            b.Total = total;
            b.PaymentMethod = paymentMethod;
            b.Status = 1;
            return await irespon.CreateItem(b);
        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateBill(Guid id, [FromBody] Bill? dm)
        {
            var lstb = await irespon.GetAll();
            var b = lstb.FirstOrDefault(x => x.BillID == id);
            if (b == null)
            {
                return false;
            }
            else
            {
                b.VoucherID = dm.VoucherID;
                b.PriceBeforeVoucher = dm.PriceBeforeVoucher;
                b.Shipmoney = dm.Shipmoney;
                b.UserPhone = dm.UserPhone;
                b.AddressUser = dm.AddressUser;
                b.OrderDate = dm.OrderDate;
                b.DeliveryDate = dm.DeliveryDate;
                b.DeliveryDate = dm.DeliveryDate;
                b.Status = dm.Status;
                return await irespon.UpdateItem(b);
            }

        }
        [HttpDelete("[Action]/{id}")]
        public async Task<bool> DeleteBill(Guid id)
        {
            var lstb = await irespon.GetAll();
            var b = lstb.FirstOrDefault(x => x.BillID == id);
            if (b == null)
            {
                return false;
            }
            else
            {
                return await irespon.DeleteItem(b);
            }
        }
    }
}

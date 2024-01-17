using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SD85_WebBookOnline.Api.Data;
using SD85_WebBookOnline.Api.IResponsitories;
using SD85_WebBookOnline.Responsitories;
using SD85_WebBookOnline.Share.Models;
namespace SD85_WebBookOnline.Api.Controllers
{
    [Route("api/Voucher")]
    [ApiController]
    public class VoucherController
    {
        private readonly IAllResponsitories<Voucher> irespon;
        AppDbContext context = new AppDbContext();
        public VoucherController()
        {
            irespon = new AllResponsitories<Voucher>(context, context.Voucher);
        }
        [Authorize]
        [HttpGet("[Action]")]
        public async Task<IEnumerable<Voucher>> GetAllVoucher()
        {
            return await irespon.GetAll();
        }
        [HttpGet("[Action]")]
        public async Task<IEnumerable<Voucher>> LayVoucherTheoDieuKien(int discountCondition)
        {
            var listVoucher = await irespon.GetAll();
            var ListVOucherByCondition = listVoucher.Where(p => p.DiscountCondition <= discountCondition && p.Status == 1).ToList();
            return ListVOucherByCondition;
        }
        [HttpGet("[Action]")]
        public async Task<Voucher> GetVoucherByVoucherCode(string VoucherCode)
        {
            var listVoucher = await irespon.GetAll();
            var ListVOucherByCondition = listVoucher.FirstOrDefault(p => p.code == VoucherCode);
            return ListVOucherByCondition;
        }

        [HttpPost("[Action]")]
        public async Task<bool> CreateVoucher(Guid createByID, Guid deletByID, string name,decimal quantity,string code, string description, DateTime starDate, DateTime endDate, decimal discountCondition, decimal discountAmount, int status)
        {
            Voucher v = new Voucher();
            v.VoucherID = Guid.NewGuid();
            v.CreateByID = createByID;
            v.DeletByID = deletByID;
            v.Name = name;
            v.Quantity = quantity;
            v.code = code;
            v.Description = description;
            v.StartDate = starDate;
            v.EndDate = endDate;
            v.DiscountCondition = discountCondition;
            v.DiscountAmount = discountAmount;
            v.DeletByID = deletByID;
            v.Status = status;
            if (v.Quantity == 0 || v.StartDate > DateTime.Now)
            {
                v.Status = 0;
            }
            return await irespon.CreateItem(v);
        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> UpdateVoucher(Guid id, [FromBody] Voucher? dm)
        {
            var lstv = await irespon.GetAll();
            var v = lstv.FirstOrDefault(x => x.VoucherID == id);
            if (v == null)
            {
                return false;
            }
            else
            {
                v.CreateByID = dm.CreateByID;
                v.DeletByID = dm.DeletByID;
                v.Name = dm.Name;
                v.Quantity = dm.Quantity;
                v.code = dm.code;
                v.Description = dm.Description;
                v.StartDate = dm.StartDate;
                v.EndDate = dm.EndDate;
                v.DiscountCondition = dm.DiscountCondition;
                v.DiscountAmount = dm.DiscountAmount;
                v.DeletByID = dm.DeletByID;
                v.Status = dm.Status;
                if (v.Quantity==0 || v.StartDate > DateTime.Now)
                {
                    v.Status = 0;
                }
                return await irespon.UpdateItem(v);
            }

        }
        [HttpPut("[Action]/{id}")]
        public async Task<bool> DeleteVoucher(Guid id)
        {
            var lstv = await irespon.GetAll();
            var v = lstv.FirstOrDefault(x => x.VoucherID == id);
            if (v == null)
            {
                return false;
            }
            else
            {
                v.Status = 0;
                return await irespon.UpdateItem(v);
            }
        }
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.Models
{
    public class Cart
    {
        public Guid CartId { get; set; }
        public Guid? VoucherID { get; set; }
        public string? UserID { get; set; }
        public decimal PriceBeforeVoucher { get; set; } // Giá trước khi add thêm mã giảm giá
        public decimal Total { get; set; }
        public int Status { get; set; }

        public virtual IEnumerable<CartItems>? CartItems { get; set; }   
        public virtual User? User { get; set; }
        public virtual Voucher? Voucher { get; set; }
    }
}

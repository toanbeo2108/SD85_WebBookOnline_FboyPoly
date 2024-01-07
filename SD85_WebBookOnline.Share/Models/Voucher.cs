using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.Models
{
    public class Voucher
    {
        public Guid VoucherID { get; set; }
        public Guid? CreateByID { get; set; }
        public Guid? DeletByID { get; set; }
        public string Name { get; set; }
        public string code { get; set; }
        public decimal Quantity { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? DiscountCondition { get; set; } // Điều kiện giảm giá
        public decimal? DiscountAmount { get; set; } // Số tiền được giảm
        
        public int Status { get; set; }

        public virtual IEnumerable<Bill>? Bill { get; set; }
        public virtual IEnumerable<Cart>? Cart { get; set; }
    }
}

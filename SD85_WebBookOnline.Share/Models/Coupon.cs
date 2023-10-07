using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.Models
{
    public class Coupon
    {
        public Guid CouponID { get; set; } // Id
        public string CouponName { get; set; } // Tên hoặc mã coupon
        public int PercentDiscount { get; set; } // Phần trăm giảm giá
        public DateTime StartDate { get; set; } // Ngày bắt đầu
        public DateTime EndDate { get; set; }// Ngày kết thúc
        public string Description { get; set; } // Mô tả
        public int Status { get; set; }

        public virtual ICollection<Book>? Books { get; set; }
    }
}

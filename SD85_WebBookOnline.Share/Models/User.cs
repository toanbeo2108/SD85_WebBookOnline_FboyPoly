using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.Models
{
    public class User : IdentityUser
    {
        public Guid? DeliveryAddressID { get; set; } // Id địa chỉ nhận hàng
        public string? Country { get; set; } // Quốc gia
        public string? Avatar { get; set; } // Link ảnh đại diện
        public DateTime? CreateDate { get; set; } // Ngày tạo
        public DateTime? UpdateDate { get; set; } // Ngày chỉnh sửa
        public string? CardNumber { get; set; } // CMT
        public int? Point { get; set; }//Điểm tích lũy
        public int? Status { get; set; } // trạng thái : 0 == Ko còn hoạt động, ẩn nick ,....

        // Set Quan hệ
        public virtual IEnumerable<Rating>? Ratings { get; set; }
        public virtual IEnumerable<DeliveryAddress>? DeliveryAddress { get; set;}
        public virtual IEnumerable<Cart>? Cart { get; set; }
        public virtual IEnumerable<Bill>? Bills { get; set; }
    }
}

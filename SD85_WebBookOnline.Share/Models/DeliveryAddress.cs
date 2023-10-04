using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.Models
{
    public class DeliveryAddress
    {
        public Guid DeliveryAddressID { get; set; }
        public string ConsigneeName { get; set; } // Tên người nhận hàng
        public string PhoneNumber { get; set; } // Sđt nhận hàng
        public string AddressLine { get; set; } // Đường
        public string City { get; set; } // Thành phố
        public string Country { get; set; } // Quốc gia
        public string Description { get; set; } // Mô tả
        public int Status { get; set; } // Trạng thái : 0 == ko hoạt động nữa ...

        // nối quan hệ
        public virtual User? User { get; set; }
    }
}

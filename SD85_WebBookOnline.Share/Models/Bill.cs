﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.Models
{
    public class Bill
    {
        public Guid BillID { get; set; }
        //public Guid? UserID { get; set; }
        public Guid? VoucherID { get; set; }
        public double PriceBeforeVoucher { get; set; } // Gía gốc
        public double Shipmoney { get; set; } // Tiền Ship
        public string UserPhone { get; set; } // SĐT Khách hàng
        public string AddressUser { get; set; } // Địa chỉ khách hàng
        public DateTime OrderDate { get; set; } // Ngày đặt hàng
        public DateTime DeliveryDate { get; set; } // Ngày giao hàng
        public double Total { get; set; } // tổng giá tiền
        public int PaymentMethod { get; set; } // Phương thức thanh toán
        public int Status { get; set; }

        public virtual IdentityUser? User { get; set; }
        public virtual Voucher? Voucher { get; set; }
        public virtual IEnumerable<BillItems>? BillItems { get; set; }

    }
}

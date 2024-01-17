using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.Models
{
    public class Book
    {
        public Guid BookID { get; set; } // Id
        public Guid? ManufacturerID { get; set; } // Id nhà xuất bản
        public Guid? FormID { get; set; } // Id Form
        public Guid? CouponID { get; set; } // Id Coupon
        public string BookName { get; set; } // Tên sách
        public int TotalQuantity { get; set; } // Tổng số lượng 
        public string MainPhoto { get; set; } // ảnh đại diện chính 
        public int QuantitySold { get; set; } // Số lượng bán được - sản lượng ...
        public int QuantityExists { get; set; } // Số lượng còn lại
        public decimal EntryPrice { get; set; } // Giá đầu vào
        public decimal Price { get; set; } // Giá bán
        public string? Information { get; set; } // Thông tin sách
        public string? Description { get; set; } // Mô tả
        public string ISBN { get; set; } // Mã định danh
        public int YearOfRelease { get; set; } // Năm phát hành
        public DateTime CreateDate { get; set; } // Ngày tạo
        public DateTime? DeleteDate { get; set; } // Ngày xóa
        public int TransactionStatus { get; set; }// trạng thái giao dịch
        public int Status { get; set; } // trạng thái sách
        public int Weight { get; set; } // cân nặng
        public decimal Volume { get; set; } //Thể tích

        // Quan hệ
        public virtual Manufacturer? Manufacturer { get; set; } 
        public virtual Form? Form { get; set; } 
        public virtual Coupon? Coupon { get; set; }
        public virtual IEnumerable<BookDetail>? BookDetails { get; set; } 
        public virtual IEnumerable<Images>? Images { get; set; }
        public virtual IEnumerable<ComboItem>? ComboItems { get; set; }
        public virtual IEnumerable<CartItems>? CartItems { get; set; }
        public virtual IEnumerable<BillItems>? BillItems { get; set; }
        public virtual IEnumerable<InputSlip>? InputSlip { get; set; }
        public virtual IEnumerable<Rating>? Ratings { get; set; }
  

    }
}

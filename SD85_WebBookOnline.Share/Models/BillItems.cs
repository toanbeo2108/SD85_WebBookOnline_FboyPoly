using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.Models
{
    public class BillItems
    {
        [Key]
        public Guid BillItemID { get; set; }
        public Guid? BookID { get; set; }
        public Guid? BillID { get; set; }
        public Guid? ComboID { get; set; }
        public string ItemName { get; set; } // Tên sản phẩm
        public decimal Price { get; set; } // Giá cả
        public decimal GiaNhap { get; set; } // Giá nhập
        public int Quantity { get; set; } // Số lượng mua
        public decimal ToTal { get; set; } // Tổng tiền
        public int Status { get; set; } 

        public virtual Book? Book { get; set; }
        public virtual Bill? Bill { get; set; }
        public virtual Combo? Combo { get; set; }

    }
}

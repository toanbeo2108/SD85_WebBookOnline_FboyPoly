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
        public string ItemName { get; set; } // Tên sản phẩm
        public double Price { get; set; } // Giá cả
        public int Quantity { get; set; } // Số lượng mua
        public double ToTal { get; set; } // Tổng tiền
        public int Status { get; set; } 

        public virtual Book? Book { get; set; }
        public virtual Bill? Bill { get; set; }

    }
}

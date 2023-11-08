using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.Models
{
    public class Combo
    {
        public Guid ComboID { get; set; }
        public Guid? CreatebyID { get; set; }
        public Guid CartItemID { get; set; }
        public string ComboName { get; set; }
        public int? Quantity { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; }
        public string Image { get; set; }
        public virtual IEnumerable<ComboItem>? ComboItems { get; set; }
        public virtual IEnumerable<CartItems>? CartItems { get; set; }
        public virtual IEnumerable<BillItems>? BillItems { get; set; }
    }
}

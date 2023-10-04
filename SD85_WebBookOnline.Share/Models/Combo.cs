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
        public double Price { get; set; }
        public int Status { get; set; }
        public virtual IEnumerable<ComboItem>? ComboItems { get; set; }
        public virtual IEnumerable<CartItems>? CartItems { get; set; }
    }
}

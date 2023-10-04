using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.Models
{
    public class ComboItem
    {
        public Guid ComboItemID { get; set; }
        public Guid? BookID { get; set; }
        public Guid? ComboID { get; set; }
        public string ItemName { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
        public double ToTal { get; set; }
        public int Status { get; set; }

        public virtual Book? Book { get; set; }
        public virtual Combo? Combo { get; set; }
    }
}

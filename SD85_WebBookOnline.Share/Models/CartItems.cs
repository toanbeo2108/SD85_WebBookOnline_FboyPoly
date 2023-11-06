using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.Models
{
    public class CartItems
    {
        [Key]
        public Guid CartItemID { get; set; }
        public Guid? CartID { get; set; }
        public Guid? ComboID { get; set; }
        public Guid? BookID { get; set; }
        public string ItemName { get; set; }
        public string? Image { get; set; }
        public decimal Price { get; set; }
        public int Quantity   { get; set; }
        public decimal ToTal { get; set; }
        public int Status { get; set; }

        public virtual Combo? Combo { get; set; }
        public virtual Book? Book { get; set; }
        public virtual Cart? Cart { get; set; }
    }
}

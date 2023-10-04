using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.Models
{
    public class Manufacturer // Nhà xuất bản
    {
        [Key]
        public Guid ManufactureID { get; set; } // ID
        public string ManufactureName { get; set; } 
        public string? Desciption { get; set; }
        public int? Status { get; set; }

        // Quan hệ
        public virtual ICollection<Book>? Books { get;}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.Models
{
    public class Rating
    {
        public Guid ID {get; set; }
        public Guid? IdBook { get; set; }
        public string? IdNguoiDung { get; set; }
        public string? Comment { get; set; }
        public int? Stars { get; set; }
        public DateTime? RatingDate { get; set; }
        public int? Status { get; set; }
        public virtual Book? Book { get; set; }
        public virtual User? User { get; set; }
    }
}

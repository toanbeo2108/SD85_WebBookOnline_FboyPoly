using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.Models
{
    public class Form
    {
        public Guid FormId { get; set; }
        public string FormName { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }

        // Quan hệ
        public virtual IEnumerable<Book>? Books { get; }
    }
}

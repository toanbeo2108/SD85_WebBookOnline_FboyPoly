using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.Models
{
    public class CategoryParent
    {
        public Guid CategoryParentID { get; set; }
        public string? CategoryParentName { get; set; }
        public int? Status { get; set; }
        public virtual IEnumerable<Category>? Categorys { get; set; }
    }
}

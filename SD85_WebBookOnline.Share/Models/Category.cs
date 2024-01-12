using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.Models
{
    public class Category
    {
        public Guid CategoryID { get; set; }
        public Guid? CategoryParentID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public virtual IEnumerable<BookDetail>? BookDetails { get; set; }
        public virtual CategoryParent? CategoryParent { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.Models
{
    public class Languge
    {
        public Guid LangugeID { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Status { get; set; }
        public virtual IEnumerable<BookDetail>? BookDetails { get; set; }

    }
}

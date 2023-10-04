using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.Models
{
    public class BookDetail
    {
        public Guid BookDetailID { get; set; }
        public Guid? BookID { get; set; }
        public Guid? CategoriesID { get; set; }
        public Guid? AuthorID { get; set; }
        public Guid? LagugeID { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Languge? Languge { get; set; }
        public virtual Author? Author { get; set; }
        public virtual Book? Book { get; set; }
    }
}

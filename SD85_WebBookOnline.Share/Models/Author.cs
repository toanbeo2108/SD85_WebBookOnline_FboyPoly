using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.Models
{
    public class Author
    {
        public Guid AuthorID { get; set; }
        public string AuthorName { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime? DateOfDie { get; set; }
        public string? Country { get; set; }
        public string? Image { get; set; }
        public string? Email { get; set; }
        public string? Bio { get; set; }
        public int? Status { get; set; }

        public virtual IEnumerable<BookDetail>? BookDetails { get; set; }

    }
}

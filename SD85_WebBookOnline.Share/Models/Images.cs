using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.Models
{
    public class Images
    {
        public Guid ImagesID { get; set; }
        public Guid? BookID { get; set; }
        public string ImageName { get; set; }
        public int Status { get; set; }
        public virtual Book? Book { get; set; }

    }
}

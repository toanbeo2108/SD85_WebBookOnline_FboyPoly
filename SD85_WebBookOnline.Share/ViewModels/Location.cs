using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.ViewModels
{
    public class Location
    {
        public class Ward
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }

        public class District
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public List<Ward> Wards { get; set; }
        }

        public class City
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public List<District> Districts { get; set; }
        }
    }
}

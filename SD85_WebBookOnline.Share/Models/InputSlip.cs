using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.Models
{
    public class InputSlip
    {
        public Guid InputSlipID { get; set; }
        public string? IdNhanVienNhap { get; set; }
        public Guid? IdSachNhap { get; set; }
        public int? SoLuong { get; set; }
        public decimal? GiaNhap { get; set; }
        public decimal? GiaBan { get; set; }
        public DateTime? NgayNhap { get; set; }
        public virtual Book? Book { get; set; }
        public virtual User? User { get; set; }


    }
}

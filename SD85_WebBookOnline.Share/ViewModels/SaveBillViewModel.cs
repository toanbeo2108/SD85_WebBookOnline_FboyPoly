using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.ViewModels
{
    public class SaveBillViewModel
    {
        [Required]
        [Phone]
        public string UserPhone { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string firstName { get; set; }

        [Required]
        public string lastName { get; set; }

        [Required]
        public string ProvinceName { get; set; }

        [Required]
        public string DistrictName { get; set; }

        [Required]
        public string WardName { get; set; }

        public string? Street { get; set; }

        [Required]
        public int PaymentMethod { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SD85_WebBookOnline.Share.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.Configurations
{
    public class BillConfigurations : IEntityTypeConfiguration<Bill>
    {
        public void Configure(EntityTypeBuilder<Bill> builder)
        {
            builder.HasKey(p => p.BillID);
            builder.HasOne(p => p.Voucher).WithMany(p => p.Bill).HasForeignKey(p => p.VoucherID);
            builder.HasOne(p => p.User).WithMany(p => p.Bills).HasForeignKey(p => p.UserID);
        }
    }
}

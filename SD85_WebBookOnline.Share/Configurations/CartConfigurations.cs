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
    public class CartConfigurations : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasKey(p => p.CartId);
            builder.HasOne(p => p.Voucher).WithMany(p => p.Cart).HasForeignKey(p => p.VoucherID);
            builder.HasOne(p => p.User).WithMany(p => p.Cart).HasForeignKey(p => p.UserID);
        }
    }
}

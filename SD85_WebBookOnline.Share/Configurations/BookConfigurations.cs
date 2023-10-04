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
    public class BookConfigurations : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(p => p.BookID);
            builder.HasOne(p => p.Manufacturer).WithMany(p => p.Books).HasForeignKey(p => p.ManufacturerID);
            builder.HasOne(p => p.Form).WithMany(p => p.Books).HasForeignKey(p => p.FormID);
            builder.HasOne(p => p.Coupon).WithMany(p => p.Books).HasForeignKey(p => p.CouponID);
        }
    }
}

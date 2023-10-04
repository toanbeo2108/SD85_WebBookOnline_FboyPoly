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
    public class BillItemConfigurations : IEntityTypeConfiguration<BillItems>
    {
        public void Configure(EntityTypeBuilder<BillItems> builder)
        {
            builder.HasKey(p => p.BillItemID);
            builder.HasOne(p => p.Book).WithMany(p => p.BillItems).HasForeignKey(p => p.BookID);
            builder.HasOne(p => p.Bill).WithMany(p => p.BillItems).HasForeignKey(p => p.BillID);
            builder.HasOne(p => p.Combo).WithMany(p => p.BillItems).HasForeignKey(p => p.ComboID);
        }
    }
}

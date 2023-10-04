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
    public class ComboItemConfigurations : IEntityTypeConfiguration<ComboItem>
    {
        public void Configure(EntityTypeBuilder<ComboItem> builder)
        {
            builder.HasKey(p => p.ComboItemID);
            builder.HasOne(p => p.Book).WithMany(p => p.ComboItems).HasForeignKey(p => p.BookID);
            builder.HasOne(p => p.Combo).WithMany(p => p.ComboItems).HasForeignKey(p => p.ComboID);
        }
    }
}

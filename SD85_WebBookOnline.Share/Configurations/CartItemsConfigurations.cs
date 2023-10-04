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
    public class CartItemsConfigurations : IEntityTypeConfiguration<CartItems>
    {
        public void Configure(EntityTypeBuilder<CartItems> builder)
        {
            builder.HasKey(p => p.CartItemID);
            builder.HasOne(p => p.Cart).WithMany(p => p.CartItems).HasForeignKey(p => p.CartID);
            builder.HasOne(p => p.Book).WithMany(p => p.CartItems).HasForeignKey(p => p.BookID);
            builder.HasOne(p => p.Combo).WithMany(p => p.CartItems).HasForeignKey(p => p.ComboID);
        }
    }
}

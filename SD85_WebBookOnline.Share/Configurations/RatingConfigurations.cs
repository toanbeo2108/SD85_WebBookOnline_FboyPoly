using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SD85_WebBookOnline.Share.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SD85_WebBookOnline.Share.Configurations
{
    public class RatingConfigurations : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.HasKey(p => p.ID);
            builder.HasOne(p => p.Book).WithMany(p => p.Ratings).HasForeignKey(p => p.IdBook);
            builder.HasOne(p => p.User).WithMany(p => p.Ratings).HasForeignKey(p => p.IdNguoiDung);
        }
    }
}

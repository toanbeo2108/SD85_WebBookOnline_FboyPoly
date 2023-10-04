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
    public class LangugeConfigurations : IEntityTypeConfiguration<Languge>
    {
        public void Configure(EntityTypeBuilder<Languge> builder)
        {
            builder.HasKey(p => p.LangugeID); // Set khóa chính
        }
    }
}

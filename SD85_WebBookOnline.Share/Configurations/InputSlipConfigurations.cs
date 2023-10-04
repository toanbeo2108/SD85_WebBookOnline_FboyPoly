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
    public class InputSlipConfigurations : IEntityTypeConfiguration<InputSlip>
    {
        public void Configure(EntityTypeBuilder<InputSlip> builder)
        {
            builder.HasKey(p => p.InputSlipID);
            builder.HasOne(p => p.Book).WithMany(p => p.InputSlip).HasForeignKey(p => p.InputSlipID);
        }
    }
}

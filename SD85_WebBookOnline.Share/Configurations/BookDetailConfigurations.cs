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
    public class BookDetailConfigurations : IEntityTypeConfiguration<BookDetail>
    {
        public void Configure(EntityTypeBuilder<BookDetail> builder)
        {
            builder.HasKey(p => p.BookDetailID); // Set khóa chính
            builder.HasOne(p => p.Category).WithMany(p => p.BookDetails).HasForeignKey(p => p.CategoriesID);
            builder.HasOne(p => p.Languge).WithMany(p => p.BookDetails).HasForeignKey(p => p.LagugeID);
            builder.HasOne(p => p.Author).WithMany(p => p.BookDetails).HasForeignKey(p => p.AuthorID);
            builder.HasOne(p => p.Book).WithMany(p => p.BookDetails).HasForeignKey(p => p.BookID);
        }
    }
}

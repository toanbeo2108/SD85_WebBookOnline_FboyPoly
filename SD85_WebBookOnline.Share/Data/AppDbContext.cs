﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SD85_WebBookOnline.Share.Models;

namespace SD85_WebBookOnline.Share.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext()
        {
            
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<CartItems> CartItems { get; set; }
        public virtual DbSet<Combo> Combo { get; set; }
        public virtual DbSet<ComboItem> ComboItem { get; set; }
        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<Form> Form { get; set; }
        public virtual DbSet<Coupon> Coupon { get; set; }
        public virtual DbSet<Manufacturer> Manufacturer { get; set; }
        public virtual DbSet<BookDetail> BookDetails { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Languge> Languges { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<PostBanner> PostBanner { get; set; }
        public virtual DbSet<InputSlip> InputSlip { get; set; }
        public virtual DbSet<BillItems> BillItems { get; set; }
        public virtual DbSet<Images> Images { get; set; }
        public virtual DbSet<Bill> Bill { get; set; }
        public virtual DbSet<Voucher> Voucher { get; set; }
        public virtual DbSet<DeliveryAddress> DeliveryAddress { get; set; }
        public virtual DbSet<CategoryParent> CategoryParents { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            CreateRoles(builder);
        }
		private void CreateRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                    new IdentityRole() { Name = "Admin", NormalizedName = "ADMIN" },
                    new IdentityRole() { Name = "User", NormalizedName = "USER" }
                );
        }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer("Data Source=LAPTOP-DAV1LO0Q\\SQLEXPRESS;Initial Catalog=WebBookOnline_DATN;Integrated Security=True;");
			}
		}
	}
}

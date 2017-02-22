using Kidshop.Areas.Admin.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kidshop.Models.BusinessModel
{
    public class KidShopDbContext:DbContext
    {
        public KidShopDbContext()
            : base("KidShop")
        {

        }
        public DbSet<Account> Account { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductImage> ProductImage { get; set; }

        public DbSet<ProductDetail> ProductDetail { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Products)
                .WithOptional(e => e.Category)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductDetails)
                .WithOptional(e => e.Product)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Product>()
                .HasMany(e => e.ProductImages)
                .WithOptional(e => e.Product)
                .WillCascadeOnDelete();
        }
    }
}
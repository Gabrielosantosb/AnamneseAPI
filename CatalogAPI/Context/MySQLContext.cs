using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Context
{
    public class MySQLContext : DbContext
    {
        public MySQLContext(DbContextOptions<MySQLContext> options) : base(options)
        {
        }

        public DbSet<Product>? Products { get; set; }
        public DbSet<Category>? Categories { get; set; }



        //Atua como FluentAPI
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            //CATEGORIA
            modelBuilder.Entity<Category>().HasKey(c => c.Id);
            modelBuilder.Entity<Category>().Property(c => c.Name)
                .HasMaxLength(255)
                .IsRequired();
            modelBuilder.Entity<Category>().Property(c => c.Description)
              .IsRequired()
              .HasMaxLength(255);

            //PRODUTO
            modelBuilder.Entity<Product>().HasKey(c => c.Id);
            modelBuilder.Entity<Product>().Property(c => c.Name).HasMaxLength(255);
            modelBuilder.Entity<Product>().Property(c => c.Description).HasMaxLength(255);
            modelBuilder.Entity<Product>().Property(c => c.Image).HasMaxLength(100);
            modelBuilder.Entity<Product>().Property(c => c.Price).
                IsRequired().
                HasPrecision(14,2);


            //RELACIONAMENTO

            modelBuilder.Entity<Product>()
                .HasOne<Category>(c => c.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(c => c.Id);
              
        }
    }
}

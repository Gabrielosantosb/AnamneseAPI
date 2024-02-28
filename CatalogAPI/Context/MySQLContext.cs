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
        public DbSet<UserModel>? Users { get; set; }




        //Atua como FluentAPI
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            #region user
            //Usuario
            modelBuilder.Entity<UserModel>().HasKey(u => u.Id);
            modelBuilder.Entity<UserModel>().Property(u => u.UserName).HasMaxLength(255).IsRequired();
            modelBuilder.Entity<UserModel>().Property(u => u.Email).HasMaxLength(255).IsRequired();
            modelBuilder.Entity<UserModel>().Property(u => u.Password).HasMaxLength(255).IsRequired();
            #endregion user

            
              
        }
    }
}

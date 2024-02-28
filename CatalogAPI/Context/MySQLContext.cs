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
        public DbSet<PacientModel>? Pacients { get; set; }





        //Atua como FluentAPI
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            #region user
            
            modelBuilder.Entity<UserModel>().HasKey(u => u.Id);
            modelBuilder.Entity<UserModel>().Property(u => u.UserName).HasMaxLength(255).IsRequired();
            modelBuilder.Entity<UserModel>().Property(u => u.Email).HasMaxLength(255).IsRequired();
            modelBuilder.Entity<UserModel>().Property(u => u.Password).HasMaxLength(255).IsRequired();
            #endregion user

            //#region pacient
            //modelBuilder.Entity<PacientModel>().HasKey(u => u.Id);
            //modelBuilder.Entity<PacientModel>().Property(u => u.UserName).HasMaxLength(255).IsRequired();
            //modelBuilder.Entity<PacientModel>().Property(u => u.Email).HasMaxLength(255).IsRequired();
            
            //#endregion pacient

        }
    }
}

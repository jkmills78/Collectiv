using Collectiv.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectiv
{
    internal class ApplicationDbContext : DbContext
    {
        public DbSet<Collection> Collection { get; set; }
        public DbSet<Item> Item { get; set; }
        public DbSet<Models.Attribute> Attribute { get; set; }
        public DbSet<File> File { get; set; }

        private readonly string connectionString;

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                "Data Source=collections.db");
            optionsBuilder.UseLazyLoadingProxies();
        }

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Collection>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Item>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Models.Attribute>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<File>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Collection>()
                .Property(b => b.Name)
                .IsRequired();

            modelBuilder.Entity<Item>()
                .Property(b => b.Name)
                .IsRequired();

            modelBuilder.Entity<Models.Attribute>()
                .Property(b => b.Name)
                .IsRequired();

            modelBuilder.Entity<Models.Attribute>()
                .Property(b => b.Value)
                .IsRequired();

            modelBuilder.Entity<File>()
                .Property(b => b.Path)
                .IsRequired();
        }
        #endregion
    }
}

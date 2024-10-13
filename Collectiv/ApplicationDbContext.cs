using Collectiv.Models;
using Microsoft.EntityFrameworkCore;
using Attribute = Collectiv.Models.Attribute;
using File = Collectiv.Models.File;

namespace Collectiv
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Container> Container { get; set; }
        public DbSet<Attribute> Attribute { get; set; }
        public DbSet<FilePackage> FilePackage { get; set; }
        public DbSet<File> File { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "collections.db");
            optionsBuilder.UseLazyLoadingProxies().UseSqlite($"Filename={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Container>()
                .HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(c => c.ParentId);
        }
    }
}

using Collectiv.Models;
using Microsoft.EntityFrameworkCore;

namespace Collectiv
{
    public class SettingsDbContext : DbContext
    {
        public DbSet<Setting> Setting { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "settings.db");
            optionsBuilder.UseLazyLoadingProxies().UseSqlite($"Filename={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Seed Data

            if(DeviceInfo.Current.Idiom == DeviceIdiom.Desktop)
                {
                modelBuilder.Entity<Setting>()
                .HasData(new Setting { Id = new Guid("10000000-0000-0000-0000-000000000000"), Name = "HostMode", Value = "Local" });
            }
            else
            {
                modelBuilder.Entity<Setting>()
                .HasData(new Setting { Id = new Guid("10000000-0000-0000-0000-000000000000"), Name = "HostMode", Value = "Hosted" });
            }

            modelBuilder.Entity<Setting>()
                .HasData(new Setting { Id = new Guid("20000000-0000-0000-0000-000000000000"), Name = "HostAddress", Value = "https://localhost:32771" });

            modelBuilder.Entity<Setting>()
                .HasData(new Setting { Id = new Guid("30000000-0000-0000-0000-000000000000"), Name = "HostAPIKey", Value = "" });

            modelBuilder.Entity<Setting>()
                .HasData(new Setting { Id = new Guid("40000000-0000-0000-0000-000000000000"), Name = "HostUsername", Value = "" });

            modelBuilder.Entity<Setting>()
                .HasData(new Setting { Id = new Guid("50000000-0000-0000-0000-000000000000"), Name = "HostPassword", Value = "" });

            #endregion
        }
    }
}

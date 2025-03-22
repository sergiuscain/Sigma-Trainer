using DBLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace DBLibrary.Data
{
    public class SigmaTrainerDbContext : DbContext
    {
        public SigmaTrainerDbContext()
        {
        }

        public DbSet<FoodRecord> FoodRecords { get; set; }
        public DbSet<DailyFoodStatistics> DailyFoodStatistics { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var pathDbSqlite = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var nameDb = "SigmaTrainerDb.db";

#if DEBUG
            nameDb = "SigmaTrainerDebug.db";
#endif

            var libraryPath = Path.Combine(pathDbSqlite, "Library");
            if (!Directory.Exists(libraryPath))
            {
                Directory.CreateDirectory(libraryPath);
            }

            var fullPath = Path.Combine(libraryPath, nameDb);
            optionsBuilder.UseSqlite($"Data Source={fullPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoodRecord>()
                .HasKey(f => f.Id);
            modelBuilder.Entity<DailyFoodStatistics>()
                .HasKey(f => f.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}

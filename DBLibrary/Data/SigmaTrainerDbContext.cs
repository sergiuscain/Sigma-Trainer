using DBLibrary.Entities;
using Microsoft.EntityFrameworkCore;

namespace DBLibrary.Data
{
    public class SigmaTrainerDbContext : DbContext
    {
        public SigmaTrainerDbContext()
        {

        }
        public DbSet<FoodRecord> FoodRecords { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            optionsBuilder.UseSqlite("Data Source=SigmaTrainerDebug.db");
#else
            optionsBuilder.UseSqlite("Data Source=SigmaTrainer.db");
#endif
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoodRecord>()
                .HasKey(f => f.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}

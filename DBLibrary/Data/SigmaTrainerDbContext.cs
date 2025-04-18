using DBLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using Sigma_Trainer.Model;

namespace DBLibrary.Data
{
    public class SigmaTrainerDbContext : DbContext
    {
        public SigmaTrainerDbContext()
        {
        }
        public DbSet<Exercises> Exercises { get; set; }
        public DbSet<DailyExerciseStatistics> DailyExerciseStaistics { get; set; }
        public DbSet<AppSettings> AppSettings { get; set; }

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

            // Настройка Exercises
            modelBuilder.Entity<Exercises>()
                .HasKey(e => e.Id);

            // Настройка DailyExerciseSatistics
            modelBuilder.Entity<DailyExerciseStatistics>()
                .HasKey(es => es.Id);

            modelBuilder.Entity<AppSettings>()
               .HasKey(a => a.Id);

            // Связи
            modelBuilder.Entity<DailyExerciseStatistics>()
                .HasOne(es => es.exercises) // DailyExerciseSatistics имеет одно Exercises
                .WithMany(e => e.ExerciseSatistics) // Exercises имеет много DailyExerciseSatistics
                .HasForeignKey(es => es.ExercisesId); // Внешний ключ в DailyExerciseSatistics

            base.OnModelCreating(modelBuilder);
        }
    }
}

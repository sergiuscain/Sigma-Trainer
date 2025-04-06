using DBLibrary.Data;
using DBLibrary.Entities;
using Microsoft.EntityFrameworkCore;

namespace Sigma_Trainer.Services
{
    public class StatisticsService
    {
        private readonly SigmaTrainerDbContext _context;
        public StatisticsService(SigmaTrainerDbContext context)
        {
            _context = context;
        }
        public async Task<List<DailyFoodStatistics>> GetFoodStatisticsAsync(int days)
        {
            var today = DateTime.Today;

            // Получаем записи статистики за последние N дней
            var statistics = await _context.DailyFoodStatistics
                .Where(stats => stats.Date >= today.AddDays(-days + 1) && stats.Date <= today)
                .ToListAsync();

            // Создаем список для хранения результатов
            var result = new List<DailyFoodStatistics>();

            // Получаем даты за последние N дней
            var dateRange = Enumerable.Range(0, days)
                .Select(i => today.AddDays(-i))
                .ToList();

            // Объединяем результаты с отсутствующими днями
            foreach (var date in dateRange)
            {
                var dailyStats = statistics.FirstOrDefault(ds => ds.Date.Date == date);
                result.Add(new DailyFoodStatistics
                {
                    Date = date,
                    Calories = dailyStats?.Calories ?? 0,
                    Proteins = dailyStats?.Proteins ?? 0,
                    Fats = dailyStats?.Fats ?? 0,
                    Carbohydrates = dailyStats?.Carbohydrates ?? 0
                });
            }

            return result;
        }
        public async Task AddExerciseStatisticsAsync(int ExerciseId, int count)
        {
            var exercise = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == ExerciseId);
            if (exercise != null)
            {
                var statistics = await _context.DailyExerciseSatistics
                    .FirstOrDefaultAsync(de => de.DateTime.Date == DateTime.Today && de.ExercisesId == ExerciseId);
                if (statistics != null)
                {
                    statistics.count += count;
                }
                else
                {
                    statistics = new DailyExerciseStatistics { count = count, DateTime = DateTime.Today, exercises = exercise };
                    _context.DailyExerciseSatistics.Add(statistics);
                }
                await _context.SaveChangesAsync();
            }
        }
        /// <summary>
        /// Берем статистику за days дней по данному упражнению. Если в какой-то из дней нету записей статистики, создаем её.
        /// </summary>
        /// <param name="ExerciseId"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<List<DailyExerciseStatistics>> GetExerciseStatisticsAsync(int ExerciseId, int size, int pageNumber)
        {
            var today = DateTime.Today;
            var lastDay = today.AddDays(-size);
            if(pageNumber > 0)
            {
                today = today.AddDays(-(size*pageNumber));
                lastDay = lastDay.AddDays(-(size*pageNumber));
            }

            // Получаем статистику за указанный период
            var statistics = await _context.DailyExerciseSatistics
                .Where(es => es.ExercisesId == ExerciseId && es.DateTime <= today && es.DateTime >= lastDay)
                .ToListAsync();

            // Создаем список для всех дней в диапазоне
            var allStatistics = new List<DailyExerciseStatistics>();

            for (var date = lastDay; date <= today; date = date.AddDays(1))
            {
                // Проверяем, есть ли запись для текущего дня
                var dailyStat = statistics.FirstOrDefault(es => es.DateTime.Date == date.Date);

                if (dailyStat != null)
                {
                    // Если запись существует, добавляем её в список
                    allStatistics.Add(dailyStat);
                }
                else
                {
                    // Если записи нет, создаем новую с нулевым значением count
                    allStatistics.Add(new DailyExerciseStatistics
                    {
                        ExercisesId = ExerciseId,
                        count = 0,
                        DateTime = date
                    });
                }
            }

            return allStatistics;
        }
        /// <summary>
        /// Берем всю статистику по данномуу упражнению
        /// </summary>
        /// <param name="ExerciseId"></param>
        /// <returns></returns>
        public async Task<List<DailyExerciseStatistics>> GetExerciseStatisticsAsync(int ExerciseId)
        {
            var today = DateTime.Today;
            var allStatistics = await _context.DailyExerciseSatistics
                .Where(es => es.exercises.Id == ExerciseId).ToListAsync();
            return allStatistics;
        }
    }
}

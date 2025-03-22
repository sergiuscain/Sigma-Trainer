using DBLibrary.Data;
using DBLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sigma_Trainer.Services
{
    public class StatisticsService
    {
        private readonly SigmaTrainerDbContext _context;
        public StatisticsService(SigmaTrainerDbContext context)
        {
            _context = context;
        }
        public async Task<List<DailyFoodStatistics>> GetFoodStatistics(int days)
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
    }
}

using DBLibrary.Data;
using DBLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Sigma_Trainer.Services
{
    public class FoodService
    {
        private readonly SigmaTrainerDbContext _context;
        public FoodService(SigmaTrainerDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Добавить запись о приеме пищи и статистику для неё. (Если статистики нету, создаем её)
        /// </summary>
        /// <param name="foodRecord"></param>
        /// <returns></returns>
        public async Task AddFoodRecordAsync(FoodRecord foodRecord)
        {
            // Добавляем новый прием пищи в базу данных
            _context.FoodRecords.Add(foodRecord);
            await _context.SaveChangesAsync();

            // Получаем дату сегодняшнего приема пищи
            var today = DateTime.Today;

            // Проверяем, существует ли запись статистики за сегодняшний день
            var dailyStats = await _context.DailyFoodStatistics
                .FirstOrDefaultAsync(stats => stats.Date.Date == today);

            if (dailyStats == null)
            {
                // Если записи нет, создаем новую
                dailyStats = new DailyFoodStatistics
                {
                    Date = today,
                    Calories = foodRecord.Calories,
                    Proteins = foodRecord.Protein,
                    Fats = foodRecord.Fats,
                    Carbohydrates = foodRecord.Carbohydrates
                };
                _context.DailyFoodStatistics.Add(dailyStats);
            }
            else
            {
                // Если запись существует, обновляем ее значения
                dailyStats.Calories += foodRecord.Calories;
                dailyStats.Proteins += foodRecord.Protein;
                dailyStats.Fats += foodRecord.Fats;
                dailyStats.Carbohydrates += foodRecord.Carbohydrates;
            }

            // Сохраняем изменения в базе данных
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Взять все приемы пищи
        /// </summary>
        /// <returns></returns>
        public async Task<List<FoodRecord>> GetFoodRecordsAsync()
        {
            var foodRecords = await _context.FoodRecords.OrderBy(fr => fr.Date).ToListAsync();
            return foodRecords;
        }
        /// <summary>
        /// Взять все приемы пищи за промежуток от сегоднищнего дня до -days
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public async Task<List<FoodRecord>> GetFoodRecordsAsync(int days)
        {
            //Сегодня
            var today = DateTime.Today;
            //Дата на days дней раньше сегоднешней
            var lastDate = today.AddDays(-days);
            //Берем записи за промежуток от сегодня до (сегодня-days)
            var foodRecords = await _context.FoodRecords
                .OrderBy(fr => fr.Date)
                .Where(fr => fr.Date >= lastDate && fr.Date <= today)
                .ToListAsync();
            return foodRecords;
        }
        /// <summary>
        /// Возвращает записи приемов пищи за день (date)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<List<FoodRecord>> GetFoodRecordsAsync(DateTime date)
        {
            DateTime startOfDay = date.Date; // Начало дня (00:00:00)
            DateTime endOfDay = startOfDay.AddDays(1).AddTicks(-1); // Конец дня (23:59:59.9999999)

            var foodRecords = await _context.FoodRecords
                .Where(fr => fr.Date >= startOfDay && fr.Date <= endOfDay) // Фильтруем по диапазону
                .OrderBy(fr => fr.Date)
                .ToListAsync();

            return foodRecords;
        }
        /// <summary>
        /// Удалить прием пищи по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteFoodRecord(int id)
        {
            var foodRecord = _context.FoodRecords.FirstOrDefault(fr => fr.Id == id);
            if (foodRecord != null)
            {
                _context.FoodRecords.Remove(foodRecord);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Обновить прием пищи
        /// </summary>
        /// <param name="foodRecord"></param>
        /// <returns></returns>
        public async Task UpdateFoodRecordAsync(FoodRecord foodRecord)
        {
            _context.FoodRecords.Update(foodRecord);
            await _context.SaveChangesAsync();
        }
        public async Task TranslateMealType(string lunch, string breakfast, string dinner, string snack)
        {
            var foodRecords = await _context.FoodRecords.ToListAsync(); // Загрузка записей в память

            foreach (var fr in foodRecords)
            {
                // Заменяем значения MealType в зависимости от текущего значения
                if (fr.MealType == "Lunch" || fr.MealType == "Mittagessen" || fr.MealType == "Обед")
                {
                    fr.MealType = lunch; // Устанавливаем значение переменной lunch
                }
                else if (fr.MealType == "Breakfast" || fr.MealType == "Frühstück" || fr.MealType == "Завтрак")
                {
                    fr.MealType = breakfast; // Устанавливаем значение переменной breakfast
                }
                else if (fr.MealType == "Dinner" || fr.MealType == "Abendessen" || fr.MealType == "Ужин")
                {
                    fr.MealType = dinner; // Устанавливаем значение переменной dinner
                }
                else if (fr.MealType == "Snack" || fr.MealType == "Перекус")
                {
                    fr.MealType = snack; // Устанавливаем значение переменной snack
                }
            }

            await _context.SaveChangesAsync(); // Сохранение изменений в базе данных
        }
    }
}

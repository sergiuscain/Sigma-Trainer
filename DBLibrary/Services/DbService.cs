using DBLibrary.Data;
using DBLibrary.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLibrary.Services
{
    public class DbService
    {
        private readonly SigmaTrainerDbContext _context;
        public DbService(SigmaTrainerDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Делает миграцию
        /// </summary>
        public void Migrate()
        {
            _context.Database.Migrate();
        }
        /// <summary>
        /// Инициализация базы данных данными по умолчанию
        /// </summary>
        public void SeedDb()
        {
            //Инициализация
        }
        /// <summary>
        /// Добавить запись о приеме пищи
        /// </summary>
        /// <param name="foodRecord"></param>
        /// <returns></returns>
        public async Task AddFoodRecord(FoodRecord foodRecord)
        {
            _context.FoodRecords.Add(foodRecord);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// Взять все приемы пищи
        /// </summary>
        /// <returns></returns>
        public async Task<List<FoodRecord>> GetFoodRecords()
        {
            var foodRecords = await _context.FoodRecords.OrderBy(fr => fr.Date).ToListAsync();
            return foodRecords;
        }
        /// <summary>
        /// Взять все приемы пищи за промежуток от сегоднищнего дня до -days
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public async Task<List<FoodRecord>> GetFoodRecords(int days)
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
        public async Task UpdateFoodRecord(FoodRecord foodRecord)
        {
            _context.FoodRecords.Update(foodRecord);
            await _context.SaveChangesAsync();
        }
    }
}

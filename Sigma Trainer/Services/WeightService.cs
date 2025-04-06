using DBLibrary.Data;
using DBLibrary.Entities;
using Microsoft.EntityFrameworkCore;


namespace Sigma_Trainer.Services
{
    public class WeightService
    {
        private readonly SigmaTrainerDbContext _context;
        public WeightService(SigmaTrainerDbContext context)
        {
            _context = context;
        }
        public async Task AddWeightAsync(WeightRecord weight)
        {
            if (weight != null)
            {
                _context.WeightRecords.Add(weight);
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteWeightAsync(int id)
        {
            var Weight = _context.WeightRecords.SingleOrDefault(wr => wr.Id == id);
            if (Weight != null)
            {
                _context.WeightRecords.Remove(Weight);
                await _context.SaveChangesAsync();
            }
        }
        public WeightRecord GetWeightAsync(int id)
        {
            return _context.WeightRecords.SingleOrDefault(wr => wr.Id == id);
        }
        public async Task<List<WeightRecord>> GetWeight(int pageSize, int pageIndex)
        {
            // Получаем сегодняшнюю дату
            var today = DateTime.Today;

            // Определяем начальную и конечную дату для выборки
            DateTime startDate = today.AddDays(-pageSize * (pageIndex + 1)); // Если pageIndex = 0, берем записи за последние pageSize дней
            DateTime endDate = today.AddDays(-pageSize * pageIndex); // Если pageIndex = 1, смещаем на pageSize дней назад

            // Получаем записи веса за указанный диапазон дат
            var weights = await _context.WeightRecords
                .Where(wr => wr.Date >= startDate && wr.Date <= endDate)
                .OrderBy(wr => wr.Date) // Сортируем по дате
                .ToListAsync();

            return weights;
        }
    }
}

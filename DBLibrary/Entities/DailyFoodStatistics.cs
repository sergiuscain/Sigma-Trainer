

namespace DBLibrary.Entities
{
    public class DailyFoodStatistics
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Calories { get; set; }
        public int Proteins { get; set; }
        public int Fats { get; set; }
        public int Carbohydrates { get; set; }
    }
}

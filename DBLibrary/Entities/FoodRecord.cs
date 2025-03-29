

namespace DBLibrary.Entities
{
    public class FoodRecord
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public MealType MealType { get; set; }
        public int Protein {  get; set; }
        public int Fats { get; set; }
        public int Carbohydrates { get; set; }
        public int Calories { get; set; }
    }
}

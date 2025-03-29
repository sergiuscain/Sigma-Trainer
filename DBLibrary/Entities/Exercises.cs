

namespace DBLibrary.Entities
{
    public class Exercises
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;

        //Связь один ко многим
        public List<DailyExerciseStatistics> ExerciseSatistics { get; set; }
    }
}

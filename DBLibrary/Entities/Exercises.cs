

namespace DBLibrary.Entities
{
    public class Exercises
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Связь один ко многим
        public List<DailyExerciseStatistics> ExerciseSatistics { get; set; }
    }
}

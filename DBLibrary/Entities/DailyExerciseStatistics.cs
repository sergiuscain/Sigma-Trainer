

namespace DBLibrary.Entities
{
    public class DailyExerciseStatistics
    {
        public int Id { get; set; }
        public int ExercisesId { get; set; }
        public int count { get; set; }
        public DateTime DateTime { get; set; }

        //Свзяь
        public Exercises exercises { get; set; }
    }
}

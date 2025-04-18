using DBLibrary.Entities;

namespace Sigma_Trainer.Services
{
    public interface IStatisticsService
    {
        Task AddExerciseStatisticsAsync(int ExerciseId, int count);
        Task<int> GetAllExerciseValue(int ExerciseId);
        Task<List<DailyExerciseStatistics>> GetExerciseStatisticsAsync(int ExerciseId);
        Task<List<DailyExerciseStatistics>> GetExerciseStatisticsAsync(int ExerciseId, int size, int pageNumber);
    }
}
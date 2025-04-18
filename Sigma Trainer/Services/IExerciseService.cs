using DBLibrary.Entities;

namespace Sigma_Trainer.Services
{
    public interface IExerciseService
    {
        Task AddExerciseAsync(Exercises exercises);
        Task DeleteExerciseAsync(int exerciseID);
        Task<Exercises> GetExerciseAsync(int exerciseID);
        Task<Exercises> GetExerciseAsync(string name);
        Task<Exercises> GetExerciseAsync(string name1, string name2, string name3);
        Task<List<Exercises>> GetExercises();
        Task RenameExerciseAsync(int exerciseId, string newName);
        Task UpdateExerciseAsync(Exercises exercises);
    }
}
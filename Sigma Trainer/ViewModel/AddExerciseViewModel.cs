using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DBLibrary.Entities;
using Sigma_Trainer.Services;

namespace Sigma_Trainer.ViewModel
{
    public partial class AddExerciseViewModel : ObservableObject
    {
        private readonly ExerciseService _exerciseService;
        [ObservableProperty]
        public string exerciseName;
        public AddExerciseViewModel(ExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }
        [RelayCommand]
        public async Task AddExercise()
        {
            if (!string.IsNullOrWhiteSpace(ExerciseName))
            {
                var exercise = new Exercises { Name = ExerciseName};
                await _exerciseService.AddExerciseAsync(exercise);
                await Shell.Current.GoToAsync("..");
            }
            ExerciseName = "";
        }
    }
}

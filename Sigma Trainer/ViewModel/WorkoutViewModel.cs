using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DBLibrary.Entities;
using Sigma_Trainer.Services;
using System.Collections.ObjectModel;

namespace Sigma_Trainer.ViewModel
{
    public partial class WorkoutViewModel : ObservableObject
    {
        private readonly StatisticsService _statisticsService;
        private readonly ExerciseService _exerciseService;
        [ObservableProperty]
        ObservableCollection<Exercises> exercises;
        public WorkoutViewModel(StatisticsService statisticsService, ExerciseService exerciseService)
        {
            _statisticsService = statisticsService;
            _exerciseService = exerciseService;
        }
        [RelayCommand]
        public async Task AddExercise()
        {
            var name = "Подтягивания";
            var description = "Упражнения крутое реально!!";
            await _exerciseService.AddExerciseAsync(new DBLibrary.Entities.Exercises { Description = description, Name = name });
        }
        [RelayCommand]
        public async Task GetExercise()
        {
            var exercises = await _exerciseService.GetExercises();
        }
        [RelayCommand]
        public async Task AddScore(int exerciseId)
        {
            var result = await Application.Current.MainPage.DisplayPromptAsync("Добавить счёт",
                "Введите количество очков:",
                "OK",
                "Отмена",
                keyboard: Keyboard.Numeric);
            // Проверяем, что пользователь ввёл значение и оно корректно преобразуется в число
            if (result != null && int.TryParse(result, out int score))
            {
                if (score > 0)
                {
                    await _statisticsService.AddExerciseStatisticsAsync(exerciseId, score);

                    var r = await _statisticsService.GetExerciseStatisticsAsync(exerciseId);
                }
            }
        }
        public async Task UpdateExerciseList()
        {
            Exercises = new ObservableCollection<Exercises>(await _exerciseService.GetExercises());
        }
    }
}

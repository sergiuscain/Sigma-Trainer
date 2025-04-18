using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DBLibrary.Entities;
using Sigma_Trainer.Resources.Languages;
using Sigma_Trainer.Services;
using Sigma_Trainer.View;
using System.Collections.ObjectModel;

namespace Sigma_Trainer.ViewModel
{
    public partial class WorkoutViewModel : ObservableObject
    {
        private readonly IStatisticsService _statisticsService;
        private readonly IExerciseService _exerciseService;
        [ObservableProperty]
        ObservableCollection<Exercises> exercises;
        public WorkoutViewModel(IStatisticsService statisticsService, IExerciseService exerciseService)
        {
            _statisticsService = statisticsService;
            _exerciseService = exerciseService;
        }
        [RelayCommand]
        public async Task AddExercise()
        {
            var viewModel = new AddExerciseViewModel(_exerciseService);
            var page = new AddExercisePage(viewModel);
            await Shell.Current.Navigation.PushAsync(page);
            await UpdateExerciseList();
        }
        [RelayCommand]
        public async Task AddScore(int exerciseId)
        {
            var result = await Application.Current.MainPage.DisplayPromptAsync(Strings.Add_a_score,
                Strings.Enter_the_number_of_points_,
                Strings.OK,
                Strings.Cancel,
                keyboard: Keyboard.Numeric);
            // Проверяем, что пользователь ввёл значение и оно корректно преобразуется в число
            if (result != null && int.TryParse(result, out int score))
            {
                if (score > 0)
                {
                    await _statisticsService.AddExerciseStatisticsAsync(exerciseId, score);
                }
            }
        }
        [RelayCommand]
        public async Task EditExercise(int exerciseId)
        {
            var newName = await Application.Current.MainPage.DisplayPromptAsync(Strings.Rename_exercise,
                Strings.Enter_a_new_name,
                Strings.OK,
                Strings.Cancel,
                keyboard: Keyboard.Text);
            // Проверяем, что пользователь ввёл значение и оно корректно преобразуется в число
            if (newName != null)
            {
                await _exerciseService.RenameExerciseAsync(exerciseId, newName);
                await UpdateExerciseList();
            }
        }
        public async Task UpdateExerciseList()
        {
            Exercises = new ObservableCollection<Exercises>(await _exerciseService.GetExercises());
        }
        [RelayCommand]
        public async Task GoToExercise(int id)
        {
            var exerciseViewModel = new ExerciseViewModel(id, _exerciseService, _statisticsService);
            var exercisePage = new ExercisePage(exerciseViewModel);
            await Shell.Current.Navigation.PushAsync(exercisePage);
        }
        public async Task InitExercises()
        {
            if ((await _exerciseService.GetExercises()).Count < 1)
            {
                var exercise1 = new Exercises { Name = Strings.Push_ups};
                var exercise2 = new Exercises { Name = Strings.Pull_ups};
                var exercise3 = new Exercises { Name = Strings.Squats};
                await _exerciseService.AddExerciseAsync(exercise1);
                await _exerciseService.AddExerciseAsync(exercise2);
                await _exerciseService.AddExerciseAsync(exercise3);
            }
        }
    }
}

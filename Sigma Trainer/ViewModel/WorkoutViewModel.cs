using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sigma_Trainer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sigma_Trainer.ViewModel
{
    public partial class WorkoutViewModel : ObservableObject
    {
        private readonly StatisticsService _statisticsService;
        private readonly ExerciseService _exerciseService;
        public WorkoutViewModel(StatisticsService statisticsService, ExerciseService exerciseService)
        {
            _statisticsService = statisticsService;
            _exerciseService = exerciseService;
        }
        [RelayCommand]
        public async Task AddExerciseAsync(string name, string description)
        {
            name = "Отжимания";
            description = "Упражнения крутое реально!!";
            await _exerciseService.AddExerciseAsync(new DBLibrary.Entities.Exercises { Description = description, Name = name });
        }
        public async Task GetExercise()
        {
            var exercises = await _exerciseService.GetExercises();
        }
    }
}

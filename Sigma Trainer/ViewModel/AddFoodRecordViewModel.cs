using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DBLibrary.Entities;
using Sigma_Trainer.Resources.Languages;
using Sigma_Trainer.Services;

namespace Sigma_Trainer.ViewModel
{
    public partial class AddFoodRecordViewModel : ObservableObject
    {
        private readonly FoodService _foodService;
        private readonly StatisticsService _statisticsService;

        [ObservableProperty]
        private int protein;

        [ObservableProperty]
        private int fat;

        [ObservableProperty]
        private int carbohydrates;

        [ObservableProperty]
        private int calories;

        [ObservableProperty]
        private string selectedMealType; // Изменено на string

        public List<string> MealTypes { get; } = new List<string>
        {
            Strings.Breakfast,
            Strings.Lunch_,
            Strings.dinner,
            Strings.Snack
        };

        public AddFoodRecordViewModel(FoodService foodService, StatisticsService statisticsService)
        {
            _foodService = foodService;
            _statisticsService = statisticsService;
            SelectedMealType = MealTypes.FirstOrDefault(); // Установка значения по умолчанию
        }

        [RelayCommand]
        public async Task AddFoodRecord()
        {
            if( selectedMealType != null)
            {
                var foodRecord = new FoodRecord
                {
                    Calories = this.Calories,
                    Carbohydrates = this.Carbohydrates,
                    Fats = this.Fat,
                    Protein = this.Protein,
                    Date = DateTime.Now,
                    MealType = SelectedMealType // Конвертация в enum
                };

                await _foodService.AddFoodRecordAsync(foodRecord);
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
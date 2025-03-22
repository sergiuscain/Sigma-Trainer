using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DBLibrary.Entities;
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
        private MealType selectedMealType;
        public AddFoodRecordViewModel(FoodService foodService, StatisticsService statisticsService)
        {
            _foodService = foodService;
            _statisticsService = statisticsService;
        }
        [RelayCommand]
        public async Task AddFoodRecord()
        {
            // Логика для сохранения данных о приеме пищи
            var foodRecord = new FoodRecord
            {
                Calories = this.Calories,
                Carbohydrates = this.Carbohydrates,
                Fats = this.Fat,
                Protein = this.Protein,
                Date = DateTime.Now,
                MealType = this.SelectedMealType
            };
            await _foodService.AddFoodRecord(foodRecord);
            await Shell.Current.GoToAsync("..");
        }
    }
}

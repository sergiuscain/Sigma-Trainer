using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sigma_Trainer.Resources.Languages;
using Sigma_Trainer.Services;
using System.Globalization;

namespace Sigma_Trainer.ViewModel
{
    public partial class SettingsViewModel : ObservableObject
    {
        public List<string> Themes { get; }
        public List<string> Languages { get; }

        [ObservableProperty]
        private string selectedTheme;

        [ObservableProperty]
        private string selectedLanguage;
        private readonly SettingsService _settingsService;
        private readonly ExerciseService _exerciseService;
        private readonly FoodService _foodService;
        public SettingsViewModel(SettingsService settingsService, ExerciseService exerciseService, FoodService foodService)
        {
            _settingsService = settingsService;
            _exerciseService = exerciseService;
            _foodService = foodService;
            Themes = new List<string> { Strings.Light, Strings.Dark, Strings.Space, Strings.Golden};
            Languages = new List<string> { "Русский", "English", "Deutsch" };
            LoadCurrentLanguage(); // Загрузка текущего языка из настроек
        }

        private void LoadCurrentLanguage()
        {
            _settingsService.LoadLanguage(); // Загружаем язык из настроек
            selectedLanguage = _settingsService.GetLanguage(); // Получаем текущий язык
        }

        [RelayCommand]
        partial void OnSelectedLanguageChanged(string selectedLanguage)
        {
            var culture = selectedLanguage switch
            {
                "Русский" => "ru-RU",
                "Deutsch" => "de-DE",
                _ => "en-US"
            };

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(culture);

            // Сохранение выбранного языка в настройках
            var settingsService = new SettingsService();
            settingsService.SetLanguage(selectedLanguage);

            //Переименовываем базовые упражнения
            var pullUp = _exerciseService.GetExerciseAsync("Pull-ups", "Подтягивания", "Klimmzüge").Result;
            if (pullUp != null)
                _exerciseService.RenameExerciseAsync(pullUp.Id, Strings.Pull_ups).Wait();

            var Push_ups = _exerciseService.GetExerciseAsync("Push-ups", "Отжимания", "Liegestütze").Result;
            if (Push_ups != null)
                _exerciseService.RenameExerciseAsync(Push_ups.Id, Strings.Push_ups).Wait();

            var Squats = _exerciseService.GetExerciseAsync("Squats", "Приседания", "Kniebeugen").Result;
            if (Squats != null)
                _exerciseService.RenameExerciseAsync(Squats.Id, Strings.Squats).Wait();

            //Переименовываем записи статистики. 
            _foodService.TranslateMealType(Strings.Lunch_, Strings.Breakfast, Strings.dinner, Strings.Snack).Wait();

            // Обновление интерфейса
            UpdateUI();
        }

        private void UpdateUI()
        {
            OnPropertyChanged(nameof(Strings));
        }

        [RelayCommand]
        partial void OnSelectedThemeChanged(string selectedTheme)
        {
            var settingsService = new SettingsService();
            settingsService.SetTheme(selectedTheme); // Сохраняем тему в настройках
            ((App)Application.Current).ApplyTheme(selectedTheme); // Применяем тему ко всему приложению
        }
    }
}

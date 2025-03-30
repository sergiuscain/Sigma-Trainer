using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        public SettingsViewModel()
        {
            Themes = new List<string> { "Светлая", "Темная", "Космос" };
            Languages = new List<string> { "Русский", "English", "Deutsch" };
            LoadCurrentLanguage(); // Загрузка текущего языка из настроек
        }

        private void LoadCurrentLanguage()
        {
            var settingsService = new SettingsService();
            settingsService.LoadLanguage(); // Загружаем язык из настроек
            selectedLanguage = settingsService.GetLanguage(); // Получаем текущий язык
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

            // Обновление интерфейса
            UpdateUI();
        }

        private void UpdateUI()
        {
            OnPropertyChanged(nameof(SelectedLanguage));
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

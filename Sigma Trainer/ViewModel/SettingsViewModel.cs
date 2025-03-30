using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sigma_Trainer.Services;

namespace Sigma_Trainer.ViewModel
{
    public partial class SettingsViewModel : ObservableObject
    {
        public List<string> Themes { get; }
        [ObservableProperty]
        private string selectedTheme;
        public SettingsViewModel()
        {
            Themes = new List<string> { "Светлая", "Темная", "Космос" };
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

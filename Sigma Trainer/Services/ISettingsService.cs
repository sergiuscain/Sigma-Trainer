
namespace Sigma_Trainer.Services
{
    public interface ISettingsService
    {
        Task<string> GetLanguageAsync();
        Task<string> GetThemeAsync();
        Task LoadLanguageAsync();
        Task LoadThemeAsync();
        void ApplyTheme(string selectedTheme);
        Task SetLanguageAsync(string language);
        Task SetThemeAsync(string theme);
    }
}
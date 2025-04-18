using DBLibrary.Data;
using Microsoft.EntityFrameworkCore;
using Sigma_Trainer.Model;
using Sigma_Trainer.Resources.Languages;
using Sigma_Trainer.Resources.Themes;
using System.Globalization;

namespace Sigma_Trainer.Services
{
    public class EFSettingsService : ISettingsService
    {
        private readonly SigmaTrainerDbContext _dbContext;

        public EFSettingsService()
        {
            _dbContext = new SigmaTrainerDbContext();
        }
        public async Task SetThemeAsync(string theme)
        {
            var settings = await _dbContext.AppSettings.FirstOrDefaultAsync();
            if (settings != null)
            {
                if (theme == Strings.Light)
                {
                    settings.SelectedTheme = ThemesEnum.Light.ToString();
                }
                else if (theme == Strings.Dark)
                {
                    settings.SelectedTheme = ThemesEnum.Dark.ToString();
                }
                else if (theme == Strings.Space)
                {
                    settings.SelectedTheme = ThemesEnum.Space.ToString();
                }
                else if (theme == Strings.Golden)
                {
                    settings.SelectedTheme = ThemesEnum.Golden.ToString();
                }
                // Можно добавить условие по умолчанию, если необходимо
                await _dbContext.SaveChangesAsync();
            }
        }

        public void ApplyTheme(string selectedTheme)
        {
            Application.Current.Resources.MergedDictionaries.Clear();

            if (selectedTheme == ThemesEnum.Light.ToString() || selectedTheme == ThemesEnum.Светлая.ToString() || selectedTheme == ThemesEnum.Hell.ToString())
            {
                Application.Current.Resources.MergedDictionaries.Add(new LightTheme());
            }
            else if (selectedTheme == ThemesEnum.Dark.ToString() || selectedTheme == ThemesEnum.Темная.ToString() || selectedTheme == ThemesEnum.Dunkel.ToString())
            {
                Application.Current.Resources.MergedDictionaries.Add(new DarkTheme());
            }
            else if (selectedTheme == ThemesEnum.Space.ToString() || selectedTheme == ThemesEnum.Космос.ToString() || selectedTheme == ThemesEnum.Raum.ToString())
            {
                Application.Current.Resources.MergedDictionaries.Add(new SpaceTheme());
            }
            else if (selectedTheme == ThemesEnum.Golden.ToString() || selectedTheme == ThemesEnum.Золотая.ToString())
            {
                Application.Current.Resources.MergedDictionaries.Add(new GoldenTheme());
            }
        }

        public async Task LoadThemeAsync()
        {
            var settings = await _dbContext.AppSettings.FirstOrDefaultAsync();
            if (settings != null)
            {
                ApplyTheme(settings.SelectedTheme);
            }
            else
            {
                await _dbContext.AppSettings.AddAsync(new AppSettings { SelectedTheme = ThemesEnum.Dark.ToString(), SelectedLanguage = "Русский" });
            }
        }

        public async Task<string> GetThemeAsync()
        {
            var settings = await _dbContext.AppSettings.FirstOrDefaultAsync();
            return settings?.SelectedTheme;
        }

        public async Task SetLanguageAsync(string language)
        {
            var settings = await _dbContext.AppSettings.FirstOrDefaultAsync();
            if (settings != null)
            {
                settings.SelectedLanguage = language;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                await _dbContext.AppSettings.AddAsync(new AppSettings { SelectedTheme = ThemesEnum.Dark.ToString(), SelectedLanguage = "Русский" });
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task LoadLanguageAsync()
        {
            var settings = await _dbContext.AppSettings.FirstOrDefaultAsync();
            if (settings != null)
            {
                var culture = settings.SelectedLanguage switch
                {
                    "Русский" => "ru-RU",
                    "Deutsch" => "de-DE",
                    _ => "en-US"
                };

                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
                CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(culture);
            }
            else
            {
                await _dbContext.AppSettings.AddAsync(new AppSettings { SelectedTheme = ThemesEnum.Dark.ToString(), SelectedLanguage = "Русский" });
                await _dbContext.SaveChangesAsync();
                await LoadLanguageAsync();
            }
        }

        public async Task<string> GetLanguageAsync()
        {
            var settings = await _dbContext.AppSettings.FirstOrDefaultAsync();
            if(settings != null)
                return settings.SelectedLanguage;

            await _dbContext.AppSettings.AddAsync(new AppSettings { SelectedTheme = ThemesEnum.Dark.ToString(), SelectedLanguage = "Русский" });
            return "Русский";
        }
    }
}

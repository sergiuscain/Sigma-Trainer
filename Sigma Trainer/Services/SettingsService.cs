﻿using Newtonsoft.Json;
using Sigma_Trainer.Model;
using Sigma_Trainer.Resources.Languages;
using Sigma_Trainer.Resources.Themes;
using System.Globalization;

namespace Sigma_Trainer.Services
{
    public class SettingsService
    {
        /// <summary>
        /// Путь к файлу с настройками
        /// </summary>
        private readonly string _settingsFilePath;
        private AppSettings _appSettings;
        public SettingsService()
        {
            _settingsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "settings.json");
            LoadOrCreateSettingsAsync().Wait();
        }
        /// <summary>
        /// Загружает в _appSettings данные из файла с настройками или создает новый файл
        /// </summary>
        private async Task LoadOrCreateSettingsAsync()
        {
            if (File.Exists(_settingsFilePath))
            {
                LoadSettings();
            }
            else
            {
                //Тема по умолчанию
                _appSettings = new AppSettings { SelectedTheme = ThemesEnum.Light.ToString(), SelectedLanguage = "Русский" };
                await SaveSettingsAsync();
            }
        }
        private void LoadSettings()
        {
            var json = File.ReadAllText(_settingsFilePath);
            _appSettings = JsonConvert.DeserializeObject<AppSettings>(json);
        }
        /// <summary>
        /// Записываает настройки из _appSettings в файл
        /// </summary>
        private async Task SaveSettingsAsync()
        {
            var json = JsonConvert.SerializeObject(_appSettings);
            await File.WriteAllTextAsync(_settingsFilePath, json);
        }
        /// <summary>
        /// Сохраняем в настройках выбранную тему
        /// </summary>
        /// <param name="theme"></param>
        /// <returns></returns>
        public async Task SetThemeAsync(string theme)
        {
            if (theme == Strings.Light)
            {
                _appSettings.SelectedTheme = ThemesEnum.Light.ToString();
            }
            else if (theme == Strings.Dark)
            {
                _appSettings.SelectedTheme = ThemesEnum.Dark.ToString();
            }
            else if (theme == Strings.Space)
            {
                _appSettings.SelectedTheme = ThemesEnum.Space.ToString();
            }
            else if (theme == Strings.Golden)
            {
                _appSettings.SelectedTheme = ThemesEnum.Golden.ToString();
            }
            await SaveSettingsAsync();
        }
        /// <summary>
        /// Загружаем тему в самом приложении и применяем её
        /// </summary>
        /// <param name="selectedTheme"></param>
        public void LoadTheme(string selectedTheme)
        {

            // Удаляем все текущие темы
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
        /// <summary>
        /// Применяем выбранную тему.
        /// </summary>
        /// <returns></returns>
        public async Task LoadTheme()
        {
            LoadTheme(_appSettings.SelectedTheme);
        }
        /// <summary>
        /// Получаем текущую тему
        /// </summary>
        /// <returns></returns>
        public string GetTheme()
        {
            LoadSettings();
            return _appSettings.SelectedTheme;
        }
        public void SetTheme(string theme)
        {
            _appSettings.SelectedTheme = theme;
            var json = JsonConvert.SerializeObject(_appSettings);
            File.WriteAllText(_settingsFilePath, json);
        }

        public void SetLanguage(string language)
        {
            _appSettings.SelectedLanguage = language;
            var json = JsonConvert.SerializeObject(_appSettings);
            File.WriteAllText(_settingsFilePath, json);
        }
        public void LoadLanguage()
        {
            LoadSettings();
            var culture = _appSettings.SelectedLanguage switch
            {
                "Русский" => "ru-RU",
                "Deutsch" => "de-DE",
                _ => "en-US"
            };

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(culture);
        }

        public string GetLanguage()
        {
            LoadSettings();
            return _appSettings.SelectedLanguage;
        }
    }
}

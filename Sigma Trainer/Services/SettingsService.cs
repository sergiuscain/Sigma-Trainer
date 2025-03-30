using Newtonsoft.Json;
using Sigma_Trainer.Model;
using Sigma_Trainer.Resources.Themes;

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
                _appSettings = new AppSettings { SelectedTheme = ThemesEnum.Темная.ToString()};
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
            _appSettings.SelectedTheme = theme;
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

            if (selectedTheme == ThemesEnum.Светлая.ToString())
            {
                Application.Current.Resources.MergedDictionaries.Add(new LightTheme());
            }
            else if (selectedTheme == ThemesEnum.Темная.ToString())
            {
                Application.Current.Resources.MergedDictionaries.Add(new DarkTheme());
            }
            else if (selectedTheme == ThemesEnum.Космос.ToString())
            {
                Application.Current.Resources.MergedDictionaries.Add(new SpaceTheme());
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
    }
}

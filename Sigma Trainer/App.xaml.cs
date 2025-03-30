using DBLibrary.Data;
using Sigma_Trainer.Resources.Themes;
using Sigma_Trainer.Services;
using System.Globalization;

namespace Sigma_Trainer
{
    public partial class App : Application
    {
        public App(SigmaTrainerDbContext context)
        {
            InitializeComponent();
            // Установка культуры по умолчанию
            var settingsService = new SettingsService();
            settingsService.LoadLanguage();
            Application.Current.RequestedThemeChanged += OnRequestedThemeChanged;
            context.Database.EnsureCreated();
        }
        private void OnRequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            var settingsService = new SettingsService();
            var selectedTheme = settingsService.GetTheme();
            ApplyTheme(selectedTheme); // Применяем сохранённую тему, а не системную
        }
        public void ApplyTheme(string selectedTheme)
        {
            // Очищаем все текущие темы
            Application.Current.Resources.MergedDictionaries.Clear();

            // Применяем выбранную тему
            switch (selectedTheme)
            {
                case "Светлая":
                    Application.Current.Resources.MergedDictionaries.Add(new LightTheme());
                    break;
                case "Темная":
                    Application.Current.Resources.MergedDictionaries.Add(new DarkTheme());
                    break;
                case "Космос":
                    Application.Current.Resources.MergedDictionaries.Add(new SpaceTheme());
                    break;
            }
        }
        protected override async void OnStart()
        {
            // Загружаем тему при запуске приложения
            var settingsService = new SettingsService();
            var selectedTheme = settingsService.GetTheme();
            ApplyTheme(selectedTheme); // Применяем тему ко всему приложению
        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
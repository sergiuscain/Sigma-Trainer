using DBLibrary.Data;
using Sigma_Trainer.Resources.Themes;
using Sigma_Trainer.Services;
using System.Globalization;

namespace Sigma_Trainer
{
    public partial class App : Application
    {
        private readonly ISettingsService _settingsService;
        public App(SigmaTrainerDbContext context, ISettingsService settingsService)
        {
            InitializeComponent();
            context.Database.EnsureCreated();
            _settingsService = settingsService;
            _settingsService.LoadLanguageAsync().Wait();
            Application.Current.RequestedThemeChanged += OnRequestedThemeChanged;
        }
        private async void OnRequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            var selectedTheme = await _settingsService.GetThemeAsync();
            ApplyTheme(selectedTheme);
        }
        public void ApplyTheme(string selectedTheme)
        {
            // Очищаем все текущие темы
            Application.Current.Resources.MergedDictionaries.Clear();

            // Применяем выбранную тему
            switch (selectedTheme)
            {
                case "Light":
                    Application.Current.Resources.MergedDictionaries.Add(new LightTheme());
                    break;
                case "Dark":
                    Application.Current.Resources.MergedDictionaries.Add(new DarkTheme());
                    break;
                case "Space":
                    Application.Current.Resources.MergedDictionaries.Add(new SpaceTheme());
                    break;
                case "Golden":
                    Application.Current.Resources.MergedDictionaries.Add(new GoldenTheme());
                    break;

                case "Светлая":
                    Application.Current.Resources.MergedDictionaries.Add(new LightTheme());
                    break;
                case "Темная":
                    Application.Current.Resources.MergedDictionaries.Add(new DarkTheme());
                    break;
                case "Космос":
                    Application.Current.Resources.MergedDictionaries.Add(new SpaceTheme());
                    break;
                case "Золотая":
                    Application.Current.Resources.MergedDictionaries.Add(new GoldenTheme());
                    break;

                case "Hell":
                    Application.Current.Resources.MergedDictionaries.Add(new LightTheme());
                    break;
                case "Dunkel":
                    Application.Current.Resources.MergedDictionaries.Add(new DarkTheme());
                    break;
                case "Raum":
                    Application.Current.Resources.MergedDictionaries.Add(new SpaceTheme());
                    break;
            }
        }
        protected override async void OnStart()
        {
            //Загружаем тему при запуске приложения
            var selectedTheme = await _settingsService.GetThemeAsync();
            ApplyTheme(selectedTheme); // Применяем тему ко всему приложению
        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
using DBLibrary.Data;
using LiveChartsCore.SkiaSharpView.Maui;
using Microsoft.Extensions.Logging;
using Sigma_Trainer.Services;
using Sigma_Trainer.View;
using Sigma_Trainer.ViewModel;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace Sigma_Trainer
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp()
                .UseLiveCharts()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            //База дынных
            builder.Services.AddDbContext<SigmaTrainerDbContext>();
            builder.Services.AddSingleton<FoodService>();
            builder.Services.AddSingleton<StatisticsService>();
            builder.Services.AddSingleton<ExerciseService>();
            //Вкладки
            builder.Services.AddSingleton<SummaryPage>();
            builder.Services.AddSingleton<SummaryViewModel>();
            builder.Services.AddSingleton<WorkoutPage>();
            builder.Services.AddSingleton<WorkoutViewModel>();
            builder.Services.AddSingleton<NutritionPage>();
            builder.Services.AddSingleton<NutritionViewModel>();
            builder.Services.AddSingleton<AddFoodRecordPage>();
            builder.Services.AddSingleton<AddFoodRecordViewModel>();
            builder.Services.AddSingleton<AddExercisePage>();
            builder.Services.AddSingleton<AddExerciseViewModel>();
            builder.Services.AddSingleton<SettingsPage>();
            builder.Services.AddSingleton<SettingsViewModel>();
            builder.Services.AddSingleton<SettingsService>();
            builder.Services.AddSingleton<WeightService>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

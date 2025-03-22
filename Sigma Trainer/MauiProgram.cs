using DBLibrary.Data;
using DBLibrary.Services;
using Microsoft.Extensions.Logging;
using Sigma_Trainer.ViewModel;

namespace Sigma_Trainer
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            //База дынных
            builder.Services.AddDbContext<SigmaTrainerDbContext>();
            builder.Services.AddSingleton<DbService>();
            //Модели представления
            builder.Services.AddSingleton<SummaryViewModel>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}

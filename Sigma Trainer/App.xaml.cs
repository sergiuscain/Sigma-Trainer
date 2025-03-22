
using DBLibrary.Services;

namespace Sigma_Trainer
{
    public partial class App : Application
    {
        public App(DbService dbService)
        {
            InitializeComponent();
            dbService.Migrate();
            dbService.SeedDb();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
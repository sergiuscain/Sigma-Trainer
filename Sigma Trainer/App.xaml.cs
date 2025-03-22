

using DBLibrary.Data;
using Sigma_Trainer.Services;

namespace Sigma_Trainer
{
    public partial class App : Application
    {
        public App(SigmaTrainerDbContext context)
        {
            InitializeComponent();
            context.Database.EnsureCreated();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
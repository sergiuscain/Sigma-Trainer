using Sigma_Trainer.View;

namespace Sigma_Trainer
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AddFoodRecordPage), typeof(AddFoodRecordPage));
        }
    }
}

using Sigma_Trainer.ViewModel;

namespace Sigma_Trainer.View;
public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage(MainViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
    }
}

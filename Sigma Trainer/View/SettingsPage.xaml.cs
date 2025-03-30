using Sigma_Trainer.ViewModel;

namespace Sigma_Trainer.View;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel viewModel)
	{
		BindingContext = viewModel;
		InitializeComponent();
	}
}
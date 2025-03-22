using Sigma_Trainer.ViewModel;

namespace Sigma_Trainer.View;

public partial class NutritionPage : ContentPage
{
	NutritionViewModel _viewModel;
	public NutritionPage(NutritionViewModel vm)
	{
		BindingContext = vm;
		_viewModel = vm;
		InitializeComponent();
	}
    protected override async void OnAppearing()
    {
	 	await _viewModel.LoadFoodRecords();
		await _viewModel.LoadStatistics();
        base.OnAppearing();
    }
}
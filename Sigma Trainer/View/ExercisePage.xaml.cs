using Sigma_Trainer.ViewModel;

namespace Sigma_Trainer.View;

public partial class ExercisePage : ContentPage
{
    private readonly ExerciseViewModel _viewModel;
	public ExercisePage(ExerciseViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
        _viewModel = vm;
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadStatistics();
    }
}
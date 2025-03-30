using Sigma_Trainer.ViewModel;

namespace Sigma_Trainer.View;

public partial class WorkoutPage : ContentPage
{
	private WorkoutViewModel _viewModel;
	public WorkoutPage(WorkoutViewModel vm)
	{
		BindingContext = vm;
		_viewModel = vm;
		InitializeComponent();
	}
    protected override async void OnAppearing()
    {
		await _viewModel.InitExercises();
	 	await _viewModel.UpdateExerciseList();
		await _viewModel.LoadStatistics();
        base.OnAppearing();
    }
}
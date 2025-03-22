using Sigma_Trainer.ViewModel;

namespace Sigma_Trainer.View;

public partial class WorkoutPage : ContentPage
{
	public WorkoutPage(WorkoutViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}
using Sigma_Trainer.ViewModel;

namespace Sigma_Trainer.View;

public partial class AddExercisePage : ContentPage
{
	public AddExercisePage(AddExerciseViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}
using Sigma_Trainer.ViewModel;

namespace Sigma_Trainer.View;

public partial class AddFoodRecordPage : ContentPage
{
	public AddFoodRecordPage(AddFoodRecordViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}
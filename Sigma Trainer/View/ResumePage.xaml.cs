using Sigma_Trainer.ViewModel;

namespace Sigma_Trainer.View;

public partial class ResumePage : ContentPage
{
	public ResumePage(ResumeViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
    private async void OnGitHubClicked(object sender, EventArgs e)
    {
        await Launcher.OpenAsync(new Uri("https://github.com/sergiuscain"));
    }
 
    private async void OnHhClicked(object sender, EventArgs e)
    {
        await Launcher.OpenAsync(new Uri("https://medyn.hh.ru/resume/a8c3bdadff0d73e5560039ed1f4a4d66723547"));
    }

    private async void OnTelegramClicked(object sender, EventArgs e)
    {
        await Launcher.OpenAsync(new Uri("https://t.me/iamrayff"));
    }
}
using CommunityToolkit.Mvvm.Input;
using Everywhere.Common;
using Everywhere.Views;
using Everywhere.Views.Pages;

namespace Everywhere.ViewModels;

public partial class AboutPageViewModel : ReactiveViewModelBase
{
    public static string Version => typeof(AboutPage).Assembly.GetName().Version?.ToString() ?? "Unknown Version";

    [RelayCommand]
    private void OpenWelcomeDialog()
    {
        // Temporarily disabled
        // DialogManager
        //     .CreateCustomDialog(ServiceLocator.Resolve<WelcomeView>())
        //     .ShowAsync();
    }

    [RelayCommand]
    private void OpenChangeLogDialog()
    {
        // Temporarily simplified - dialog disabled
        // DialogManager
        //     .CreateDialog(ServiceLocator.Resolve<ChangeLogView>())
        //     .Dismissible()
        //     .Show();
    }
}
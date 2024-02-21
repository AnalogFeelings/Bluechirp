using Bluechirp.Library.Enums;
using Bluechirp.Library.Models.View;
using Bluechirp.Library.Services.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;

namespace Bluechirp.Views;

public sealed partial class ShellPage : Page
{
    private INavigationService _navigationService;

    public ShellViewModel ViewModel => (ShellViewModel)this.DataContext;

    public ShellPage()
    {
        this.InitializeComponent();
        this.DataContext = App.ServiceProvider.GetRequiredService<ShellViewModel>();

        _navigationService = App.ServiceProvider.GetRequiredService<INavigationService>();

        _navigationService.TargetFrame = ContentFrame;
        AppTitleBar.Window = MainWindow.Current;
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {

    }

    private void NavigationViewControl_Loaded(object sender, RoutedEventArgs e)
    {
        NavigationViewControl.SelectedItem = NavigationViewControl.MenuItems[0];

        _navigationService.Navigate(PageType.Timeline, TimelineType.Home, new EntranceNavigationTransitionInfo());
    }

    private void AppTitleBar_BackButtonClick(object sender, RoutedEventArgs e)
    {
        if (_navigationService.TargetFrame.CanGoBack)
            _navigationService.TargetFrame.GoBack();
    }

    private void AppTitleBar_PaneButtonClick(object sender, RoutedEventArgs e)
    {
        NavigationViewControl.IsPaneOpen = !NavigationViewControl.IsPaneOpen;
    }
}
using Bluechirp.Library.Enums;
using Bluechirp.Library.Services.Interface;
using Bluechirp.Library.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;

namespace Bluechirp.Views
{
    public sealed partial class ShellPage : Page
    {
        private INavigationService _navigationService;

        public ShellViewModel ViewModel => (ShellViewModel)this.DataContext;

        public ShellPage()
        {
            this.InitializeComponent();
            this.DataContext = App.ServiceProvider.GetRequiredService<ShellViewModel>();

            _navigationService = App.ServiceProvider.GetRequiredService<INavigationService>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainWindow.Current.Activated += MainWindow_Activated;

            _navigationService.TargetFrame = ContentFrame;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            MainWindow.Current.Activated -= MainWindow_Activated;
        }

        private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
        {
            if (args.WindowActivationState == WindowActivationState.Deactivated)
                AppTitle.Foreground = App.Current.Resources["WindowCaptionForegroundDisabled"] as SolidColorBrush;
            else
                AppTitle.Foreground = App.Current.Resources["WindowCaptionForeground"] as SolidColorBrush;
        }

        private void NavigationViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationViewControl.SelectedItem = NavigationViewControl.MenuItems[0];

            _navigationService.Navigate(PageType.HomeTimeline, new EntranceNavigationTransitionInfo());
        }
    }
}

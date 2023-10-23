using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

namespace Bluechirp.Views
{
    public sealed partial class ShellPage : Page
    {
        public ShellPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainWindow.Current.Activated += MainWindow_Activated;
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
    }
}

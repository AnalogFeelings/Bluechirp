using Bluechirp.Views;
using Microsoft.UI;
using WinUIEx;

namespace Bluechirp
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : WindowEx
    {
        public MainWindow()
        {
            this.InitializeComponent();

            this.MinWidth = 320;
            this.MinHeight = 569;

            AppWindow.Title = "Bluechirp";
            AppWindow.TitleBar.ExtendsContentIntoTitleBar = true;
            AppWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
            AppWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        }

        public void ShowSplash()
        {
            ContentFrame.Navigate(typeof(SplashScreenPage));
        }
    }
}

using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bluechirp.View
{
    /// <summary>
    /// The page code for the Login page.
    /// </summary>
    public sealed partial class LoginView : Page
    {
        /// <summary>
        /// Creates a new instance of the <see cref="LoginView"/> class.
        /// </summary>
        public LoginView()
        {
            InitializeComponent();

            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            ApplicationViewTitleBar viewTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.ButtonBackgroundColor = Color.FromArgb(0, 0, 0, 0);

            Window.Current.SetTitleBar(AppTitleBar);
        }
    }
}
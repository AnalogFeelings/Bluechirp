using Bluechirp.Library.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Bluechirp.Views
{
    public sealed partial class LoginPage : Page
    {
        public LoginViewModel ViewModel => (LoginViewModel)this.DataContext;

        public LoginPage()
        {
            this.InitializeComponent();
            this.DataContext = App.ServiceProvider.GetRequiredService<LoginViewModel>();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ViewModel.Dispose();
        }
    }
}

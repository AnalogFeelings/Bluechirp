using Bluechirp.Library.Models.View.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace Bluechirp.Views.Navigation;

/// <summary>
/// Contains application settings and information about
/// the application itself.
/// </summary>
public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel => (SettingsViewModel)this.DataContext;

    public SettingsPage()
    {
        this.InitializeComponent();
        this.DataContext = App.ServiceProvider.GetRequiredService<SettingsViewModel>();
    }
}

using Bluechirp.Library.Commands;
using Bluechirp.Library.Helpers;
using Bluechirp.Library.Services;
using Bluechirp.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Windows.System;

namespace Bluechirp.ViewModel
{
    /// <summary>
    /// The view model for <see cref="LoginView"/> page.
    /// </summary>
    internal partial class LoginViewModel : ObservableObject
    {
        /// <summary>
        /// The instance URL in the login text box.
        /// </summary>
        [ObservableProperty]
        private string _instanceUrl;
        private NavService _navService;

        /// <summary>
        /// Creates a new instance of the <see cref="LoginViewModel"/> class.
        /// </summary>
        public LoginViewModel()
        {
            _navService = App.Services.GetRequiredService<NavService>();

            AuthHelper.AuthCompleted += AuthHelper_AuthCompleted;
        }

        /// <summary>
        /// Helper function that finishes authentication work.
        /// </summary>
        /// <param name="Sender">Sender object.</param>
        /// <param name="E">Event arguments.</param>
        private void AuthHelper_AuthCompleted(object Sender, EventArgs E)
        {
            AuthHelper.AuthCompleted -= AuthHelper_AuthCompleted;
            _navService.Navigate(typeof(ShellView));
        }

        /// <summary>
        /// Logs in to the instance defined in <see cref="InstanceUrl"/>.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        [RelayCommand]
        private async Task LoginAsync()
        {
            if (InstanceMatchService.CheckIfInstanceNameIsProperlyFormatted(InstanceUrl))
            {
                await AuthHelper.Instance.LoginAsync(InstanceUrl);
            }
            else
            {
                InstanceUrl = string.Empty;
                await ErrorService.ShowInstanceUrlFormattingError();
            }
        }

        /// <summary>
        /// Opens a browser to the sign up page of the instance defined in <see cref="InstanceUrl"/>.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        [RelayCommand]
        private async Task SignUpAsync()
        {
            if (InstanceMatchService.CheckIfInstanceNameIsProperlyFormatted(InstanceUrl))
            {
                // Concat the URIs safely.
                Uri baseUri = new Uri($"https://{InstanceUrl}");

                await Launcher.LaunchUriAsync(new Uri(baseUri, "auth/sign_up"));
            }
            else
            {
                InstanceUrl = string.Empty;
                await ErrorService.ShowInstanceUrlFormattingError();
            }
        }
    }
}
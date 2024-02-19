using Bluechirp.Library.Enums;
using Bluechirp.Library.Services.Interface;
using Bluechirp.Library.Services.Security;
using Bluechirp.Library.Services.Utility;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using Windows.System;

namespace Bluechirp.Library.Models.View
{
    /// <summary>
    /// Implements a view model for a login page.
    /// </summary>
    public partial class LoginViewModel : ObservableObject, IDisposable
    {
        [ObservableProperty]
        private string _instanceUrl;

        private IInstanceUtilityService _instanceUtils;
        private INavigationService _navigationService;
        private IAuthService _authService;

        public LoginViewModel(INavigationService navigationService,
                              IAuthService authService, 
                              IInstanceUtilityService instanceUtils)
        {
            _instanceUtils = instanceUtils;
            _navigationService = navigationService;
            _authService = authService;

            _authService.OnAuthCompleted += AuthService_OnAuthCompleted;
        }

        /// <summary>
        /// Logs in to the instance defined in <see cref="ViewModel.LoginViewModel.InstanceUrl"/>.
        /// </summary>
        [RelayCommand]
        private async Task LoginAsync()
        {
            if (_instanceUtils.CheckInstanceName(InstanceUrl))
            {
                string rawUrl = await _authService.CreateAuthUrlAsync(InstanceUrl);
                Uri oauthUri = new Uri(rawUrl);

                await Launcher.LaunchUriAsync(oauthUri);
            }
            else
            {
                InstanceUrl = string.Empty;
            }
        }

        /// <summary>
        /// Opens a browser to the sign up page of the instance defined in <see cref="ViewModel.LoginViewModel.InstanceUrl"/>.
        /// </summary>
        [RelayCommand]
        private async Task SignUpAsync()
        {
            if (_instanceUtils.CheckInstanceName(InstanceUrl))
            {
                // Concat the URIs safely.
                Uri baseUri = new Uri($"https://{InstanceUrl}");

                await Launcher.LaunchUriAsync(new Uri(baseUri, "auth/sign_up"));
            }
            else
            {
                InstanceUrl = string.Empty;
            }
        }

        private void AuthService_OnAuthCompleted(object sender, EventArgs e)
        {
            _navigationService.Navigate(PageType.Shell);
        }

        public void Dispose()
        {
            _authService.OnAuthCompleted -= AuthService_OnAuthCompleted;
        }
    }
}

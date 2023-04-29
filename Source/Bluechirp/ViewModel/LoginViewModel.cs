using System;
using System.Threading.Tasks;
using Windows.System;
using Bluechirp.Library.Commands;
using Bluechirp.Library.Helpers;
using Bluechirp.Library.Model;
using Bluechirp.Library.Services;
using Bluechirp.View;

namespace Bluechirp.ViewModel
{
    /// <summary>
    /// The view model for <see cref="LoginView"/> page.
    /// </summary>
    internal class LoginViewModel : Notifier
    {
        private string _InstanceUrl;

        /// <summary>
        /// The instance URL in the login text box.
        /// </summary>
        public string InstanceUrl
        {
            get => _InstanceUrl;
            set
            {
                _InstanceUrl = value;
                NotifyPropertyChanged();
            }
        }

        public readonly RelayCommand LoginCommand;
        public readonly RelayCommand SignUpCommand;

        /// <summary>
        /// Creates a new instance of the <see cref="LoginViewModel"/> class.
        /// </summary>
        public LoginViewModel()
        {
            AuthHelper.AuthCompleted += AuthHelper_AuthCompleted;

            LoginCommand = new RelayCommand(async () => await LoginAsync());
            SignUpCommand = new RelayCommand(async () => await SignUpAsync());
        }

        /// <summary>
        /// Helper function that finishes authentication work.
        /// </summary>
        /// <param name="Sender">Sender object.</param>
        /// <param name="E">Event arguments.</param>
        private void AuthHelper_AuthCompleted(object Sender, EventArgs E)
        {
            AuthHelper.AuthCompleted -= AuthHelper_AuthCompleted;
            NavService.Instance.Navigate(typeof(ShellView));
        }

        /// <summary>
        /// Logs in to the instance defined in <see cref="InstanceUrl"/>.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        private async Task LoginAsync()
        {
            if (InstanceMatchService.CheckIfInstanceNameIsProperlyFormatted(_InstanceUrl))
            {
                await AuthHelper.Instance.LoginAsync(_InstanceUrl);
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
        private async Task SignUpAsync()
        {
            if (InstanceMatchService.CheckIfInstanceNameIsProperlyFormatted(_InstanceUrl))
            {
                // Concat the URIs safely.
                Uri baseUri = new Uri($"https://{_InstanceUrl}");

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
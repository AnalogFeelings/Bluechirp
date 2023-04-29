using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Bluechirp.View;
using Bluechirp.Library.Commands;
using Bluechirp.Library.Helpers;
using Bluechirp.Library.Model;
using Bluechirp.Library.Services;

namespace Bluechirp.ViewModel
{
    class LoginViewModel : Notifier
    {
        private string _instanceURL;

        public string InstanceURL
        {
            get { return _instanceURL; }
            set
            {
                _instanceURL = value;
                NotifyPropertyChanged();
            }
        }

        public RelayCommand LoginCommand;
        public RelayCommand SignUpCommand;

        public LoginViewModel()
        {
            AuthHelper.AuthCompleted += AuthHelper_AuthCompleted;
            LoginCommand = new RelayCommand(async () => await LoginAsync());
            SignUpCommand = new RelayCommand(async () => await SignUpAsync());
        }

        private void AuthHelper_AuthCompleted(object sender, EventArgs e)
        {
            AuthHelper.AuthCompleted -= AuthHelper_AuthCompleted;
            NavService.Instance.Navigate(typeof(ShellView));
        }

        private async Task LoginAsync()
        {
            if (InstanceMatchService.CheckIfInstanceNameIsProperlyFormatted(_instanceURL))
            {
                await AuthHelper.Instance.LoginAsync(_instanceURL);
            }
            else
            {
                InstanceURL = string.Empty;
                await ErrorService.ShowInstanceUrlFormattingError();
            }
        }

        private async Task SignUpAsync()
        {
            if (InstanceMatchService.CheckIfInstanceNameIsProperlyFormatted(_instanceURL))
            {
                // Concat the URIs safely.
                Uri baseUri = new Uri($"https://{_instanceURL}");

                await Launcher.LaunchUriAsync(new Uri(baseUri, "auth/sign_up"));
            }
            else
            {
                InstanceURL = string.Empty;
                await ErrorService.ShowInstanceUrlFormattingError();
            }
        }
    }
}

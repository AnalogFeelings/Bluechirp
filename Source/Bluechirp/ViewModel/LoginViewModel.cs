using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public LoginViewModel()
        {
            AuthHelper.AuthCompleted += AuthHelper_AuthCompleted;
            LoginCommand = new RelayCommand(async () => await LoginAsync());
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
    }
}

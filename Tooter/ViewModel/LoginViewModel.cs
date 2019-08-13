using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Commands;
using Tooter.Helpers;
using Tooter.Model;
using Tooter.Services;

namespace Tooter.ViewModel
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
            LoginCommand = new RelayCommand(async () => await LoginAsync());
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

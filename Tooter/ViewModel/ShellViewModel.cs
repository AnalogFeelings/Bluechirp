using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Commands;
using Tooter.Dialogs;
using Tooter.Helpers;
using Tooter.Model;
using Tooter.Services;
using Tooter.View;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Tooter.ViewModel
{
    class ShellViewModel : Notifier
    {

        private Account _currentUser;

        public Account CurrentUser
        {
            get { return _currentUser; }
            set {
                _currentUser = value;
                NotifyPropertyChanged();
            }
        }

        public RelayCommand NewTootCommand;

        public ShellViewModel()
        {
            NewTootCommand = new RelayCommand(async () => await NavigateToTootView());
        }

        private async Task NavigateToTootView()
        {
            await new NewTootDialog(CurrentUser.StaticAvatarUrl).ShowAsync();
        }


        internal async Task DoAsyncPrepartions()
        {
            CurrentUser = await ClientHelper.Client.GetCurrentUser();
        }

        internal async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            await ClientHelper.Logout();
        }

    }
}

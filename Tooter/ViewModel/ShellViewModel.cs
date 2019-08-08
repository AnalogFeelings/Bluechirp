using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Commands;
using Tooter.Dialogs;
using Tooter.Model;
using Tooter.Services;
using Tooter.View;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Tooter.ViewModel
{
    class ShellViewModel : Notifier
    {
        public RelayCommand NewTootCommand;

        public ShellViewModel()
        {
            NewTootCommand = new RelayCommand(async () => await NavigateToTootView());
        }

        private async Task NavigateToTootView()
        {
            await new NewTootDialog().ShowAsync();
        }


        
    }
}

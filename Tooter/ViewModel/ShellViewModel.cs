using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Commands;
using Tooter.Model;
using Tooter.Services;
using Tooter.View;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Tooter.ViewModel
{
    class ShellViewModel : Notifier
    {
        public ShellViewModel()
        {
            NewTootCommand = new RelayCommand(() => NavigateToTootView());
        }

        private void NavigateToTootView()
        {
            NavService.CreateInstance(Window.Current.Content as Frame);
            NavService.Instance.Navigate(typeof(TootView));
        }

        public RelayCommand NewTootCommand;

        
    }
}

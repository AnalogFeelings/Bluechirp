using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Model;
using Tooter.Services;
using Tooter.View;

namespace Tooter.ViewModel
{
    class ShellViewModel : Notifier
    {
        public ShellViewModel()
        {
            NavService.Instance.Navigate(typeof(TimelineView), new HomeViewModel());
        }
    }
}

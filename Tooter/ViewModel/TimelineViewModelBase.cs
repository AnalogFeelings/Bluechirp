using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Commands;
using Tooter.Model;

namespace Tooter.ViewModel
{
    public abstract class TimelineViewModelBase : Notifier
    {
        public ObservableCollection<object> TootTimelineCollection { get; set; }
         
        public abstract string ViewTitle { get; protected set; }

        public RelayCommandWithParameter DeleteCommand;

        public TimelineViewModelBase()
        {
            DeleteCommand = new RelayCommandWithParameter(DeleteToot);
        }

        private void DeleteToot(object tootToDelete)
        {
            // Delete toot from the network then delete the toot locally.
            TootTimelineCollection.Remove(tootToDelete);
        }
    }
}

using Mastonet.Entities;
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

        protected long? previousPageSinceId;
        protected long? previousPageMinId;
        protected long? nextPageMaxId;

        public abstract event EventHandler TootsAdded;

        private ObservableCollection<Status> _tootTimelineCollection;

        public ObservableCollection<Status> TootTimelineCollection
        {
            get { return _tootTimelineCollection; }
            set
            {
                _tootTimelineCollection = value;
                NotifyPropertyChanged();
            }
        }



        protected MastodonList<Status> tootTimelineData;

        public abstract string ViewTitle { get; protected set; }

        public RelayCommandWithParameter DeleteCommand;

        public TimelineViewModelBase()
        {
            //TootTimelineCollection = new MastodonList<Status>();
            DeleteCommand = new RelayCommandWithParameter(DeleteToot);
        }

        private void DeleteToot(object obj)
        {
            if (obj is Status tootToDelete)
            {
                TootTimelineCollection.Remove(tootToDelete);
            }
        }

        internal abstract Task LoadFeedAsync();

        internal abstract Task AddOlderContentToFeed();

        internal abstract Task AddNewerContentToFeed();



    }
}

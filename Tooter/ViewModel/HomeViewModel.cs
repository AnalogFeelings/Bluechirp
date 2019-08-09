using Mastonet;
using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Helpers;
using Tooter.Model;

namespace Tooter.ViewModel
{
    class HomeViewModel : TimelineViewModelBase
    {
        public override string ViewTitle { get; protected set; } = "Home";

        public HomeViewModel() : base()
        {

        }

        public override event EventHandler TootsAdded;

        internal async override Task LoadFeedAsync()
        {
            tootTimelineData = await ClientHelper.Client.GetHomeTimeline();
            TootTimelineCollection = new ObservableCollection<Status>(tootTimelineData);
        }

        internal async override Task AddOlderContentToFeed()
        {
            var olderContent = await ClientHelper.Client.GetHomeTimeline(tootTimelineData.NextPageMaxId);
            tootTimelineData.AddRange(olderContent);
            foreach (var item in olderContent)
            {
                TootTimelineCollection.Add(item);
            }

            TootsAdded?.Invoke(null, EventArgs.Empty);
        }

        internal override Task AddNewerContentToFeed()
        {
            throw new NotImplementedException();
        }
    }
}

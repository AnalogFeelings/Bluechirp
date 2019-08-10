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
            nextPageMaxId = tootTimelineData.NextPageMaxId;
            previousPageMinId = tootTimelineData.PreviousPageMinId;
            previousPageSinceId = tootTimelineData.PreviousPageSinceId;

            TootTimelineCollection = new ObservableCollection<Status>(tootTimelineData);
        }

        internal async override Task AddOlderContentToFeed()
        {
            var olderContent = await ClientHelper.Client.GetHomeTimeline(nextPageMaxId);
            nextPageMaxId = olderContent.NextPageMaxId;

            tootTimelineData.AddRange(olderContent);
            foreach (var item in olderContent)
            {
                TootTimelineCollection.Add(item);
            }

            TootsAdded?.Invoke(null, EventArgs.Empty);
        }

        internal async override Task AddNewerContentToFeed()
        {
            Mastonet.ArrayOptions options = new Mastonet.ArrayOptions();
            
            options.MinId = previousPageSinceId;
            
            var newerContent = await ClientHelper.Client.GetHomeTimeline(options);

            previousPageMinId = newerContent.PreviousPageMinId;
            previousPageSinceId = newerContent.PreviousPageSinceId;

            tootTimelineData.InsertRange(0, newerContent);

            for (int i = newerContent.Count - 1; i > -1; i--)
            {
                TootTimelineCollection.Insert(0, newerContent[i]);
            }

            TootsAdded?.Invoke(null, EventArgs.Empty);
        }
    }
}

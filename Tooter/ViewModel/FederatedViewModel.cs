using Mastonet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Helpers;

namespace Tooter.ViewModel
{
    class FederatedViewModel : TimelineViewModelBase
    {
        public override string ViewTitle { get; protected set; } = "Federated Timeline";

        public override event EventHandler TootsAdded;

        internal override Task AddNewerContentToFeed()
        {
            throw new NotImplementedException();
        }

        internal async override Task AddOlderContentToFeed()
        {
            var options = new ArrayOptions();
            options.MaxId = nextPageId;
            var olderContent = await ClientHelper.Client.GetPublicTimeline(options);
            nextPageId = olderContent.NextPageMaxId;

            tootTimelineData.AddRange(olderContent);
            foreach (var item in olderContent)
            {
                TootTimelineCollection.Add(item);
            }

            TootsAdded?.Invoke(null, EventArgs.Empty);
        }

        internal async override Task LoadFeedAsync()
        {
            base.tootTimelineData = await ClientHelper.Client.GetPublicTimeline();
            nextPageId = tootTimelineData.NextPageMaxId;
            previousPageId = tootTimelineData.PreviousPageSinceId;

            TootTimelineCollection = new System.Collections.ObjectModel.ObservableCollection<Mastonet.Entities.Status>(tootTimelineData);
        }
    }
}

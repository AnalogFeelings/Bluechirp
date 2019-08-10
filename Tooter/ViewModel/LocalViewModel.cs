using Mastonet;
using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Helpers;
using Tooter.Model;

namespace Tooter.ViewModel
{
    class LocalViewModel : TimelineViewModelBase
    {
        public override string ViewTitle { get; protected set; } = "Local Timeline";

        public override event EventHandler TootsAdded;


        

        internal async override Task AddNewerContentToFeed()
        {
            Mastonet.ArrayOptions options = new Mastonet.ArrayOptions();

            options.MinId = previousPageMinId;
            options.SinceId = previousPageSinceId;

            var newerContent = await ClientHelper.Client.GetPublicTimeline(options, true);

            previousPageMinId = newerContent.PreviousPageMinId;
            previousPageSinceId = newerContent.PreviousPageSinceId;

            tootTimelineData.InsertRange(0, newerContent);

            for (int i = newerContent.Count - 1; i > -1; i--)
            {
                TootTimelineCollection.Insert(0, newerContent[i]);
            }

            TootsAdded?.Invoke(null, EventArgs.Empty);
        }

        internal async override Task AddOlderContentToFeed()
        {
            var options = new ArrayOptions();
            options.MaxId = nextPageMaxId;
            var olderContent = await ClientHelper.Client.GetPublicTimeline(options, true);
            nextPageMaxId = olderContent.NextPageMaxId;

            tootTimelineData.AddRange(olderContent);
            foreach (var item in olderContent)
            {
                TootTimelineCollection.Add(item);
            }

            TootsAdded?.Invoke(null, EventArgs.Empty);
        }

        internal override void CacheTimeline(Status currentTopVisibleStatus)
        {
            throw new NotImplementedException();
        }

        internal async override Task LoadFeedAsync()
        {
            base.tootTimelineData = await ClientHelper.Client.GetPublicTimeline(local: true);
            nextPageMaxId = tootTimelineData.NextPageMaxId;
            previousPageMinId = tootTimelineData.PreviousPageMinId;
            previousPageSinceId = tootTimelineData.PreviousPageSinceId;

            TootTimelineCollection = new System.Collections.ObjectModel.ObservableCollection<Mastonet.Entities.Status>(tootTimelineData);
        }
    }
}

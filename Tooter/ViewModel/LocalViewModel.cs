using Mastonet;
using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Enums;
using Tooter.Helpers;
using Tooter.Model;
using Tooter.Services;

namespace Tooter.ViewModel
{
    class LocalViewModel : TimelineViewModelBase
    {
        public override string ViewTitle { get; protected set; } = "Local Timeline";

        public override event EventHandler TootsAdded;
        public override event EventHandler<Status> StatusMarkerAdded;

        protected override Task<bool> AttemptToLoadFromCache()
        {
            throw new NotImplementedException();
        }

        internal async override Task AddNewerContentToFeed()
        {
            Mastonet.ArrayOptions options = new Mastonet.ArrayOptions();

            options.MinId = previousPageMinId;
            options.SinceId = previousPageSinceId;

            try
            {
                var newerContent = await ClientHelper.Client.GetPublicTimeline(options, true);


                if (newerContent.Count > 0)
                {
                    previousPageMinId = newerContent.PreviousPageMinId;
                    previousPageSinceId = newerContent.PreviousPageSinceId;

                    tootTimelineData.InsertRange(0, newerContent);

                    for (int i = newerContent.Count - 1; i > -1; i--)
                    {
                        TootTimelineCollection.Insert(0, newerContent[i]);
                    }
                }
            }
            catch (Exception)
            {
                await ErrorService.ShowConnectionError();
                
            }

            TootsAdded?.Invoke(null, EventArgs.Empty);
        }

        internal async override Task AddOlderContentToFeed()
        {
            var options = new ArrayOptions();
            options.MaxId = nextPageMaxId;

            try
            {
                var olderContent = await ClientHelper.Client.GetPublicTimeline(options, true);
                nextPageMaxId = olderContent.NextPageMaxId;

                tootTimelineData.AddRange(olderContent);
                foreach (var item in olderContent)
                {
                    TootTimelineCollection.Add(item);
                }
            }
            catch (Exception)
            {

                await ErrorService.ShowConnectionError();
            }

            TootsAdded?.Invoke(null, EventArgs.Empty);
        }

        internal async override Task CacheTimeline(Status currentTopVisibleStatus)
        {
            var timelineSettings = new TimelineSettings(nextPageMaxId, previousPageMinId, previousPageSinceId, TimelineType.Local);
            await CacheService.CacheTimeline(tootTimelineData, currentTopVisibleStatus, timelineSettings);
        }

        internal async override Task LoadFeedAsync()
        {
            try
            {
                base.tootTimelineData = await ClientHelper.Client.GetPublicTimeline(local: true);
                nextPageMaxId = tootTimelineData.NextPageMaxId;
                previousPageMinId = tootTimelineData.PreviousPageMinId;
                previousPageSinceId = tootTimelineData.PreviousPageSinceId;

                TootTimelineCollection = new System.Collections.ObjectModel.ObservableCollection<Mastonet.Entities.Status>(tootTimelineData);
            }
            catch (Exception)
            {

                await ErrorService.ShowConnectionError();
            }
        }
    }
}

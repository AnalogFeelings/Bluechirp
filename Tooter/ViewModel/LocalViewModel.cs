using Mastonet;
using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooterLib.Enums;
using TooterLib.Helpers;
using TooterLib.Model;
using Tooter.Services;
using TooterLib.Services;

namespace Tooter.ViewModel
{
    class LocalViewModel : TimelineViewModelBase
    {
        public override string ViewTitle { get; protected set; } = "Local Timeline";

        public override event EventHandler TootsAdded;
        public override event EventHandler<Status> StatusMarkerAdded;

        protected async override Task<bool> AttemptToLoadFromCache()
        {
            bool cacheWasLoaded = false;
            var cacheLoadResult = await CacheService.LoadTimelineCache(TimelineType.Local);
            if (cacheLoadResult.wasTimelineLoaded)
            {
                var cache = cacheLoadResult.cacheToReturn;
                if (cache.Toots.Count > 0)
                {
                    StatusMarkerAdded?.Invoke(this, cache.CurrentStatusMarker);
                    tootTimelineData = cache.Toots;
                    TootTimelineCollection = new ObservableCollection<Status>(tootTimelineData);
                    previousPageMinId = cache.CurrentTimelineSettings.PreviousPageMinID;
                    previousPageSinceId = cache.CurrentTimelineSettings.PreviousPageSinceID;
                    nextPageMaxId = cache.CurrentTimelineSettings.NextPageMaxID;
                    cacheWasLoaded = true;
                }
            }

            return cacheWasLoaded;
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
            bool wasFeedloadedFromCache = await AttemptToLoadFromCache();

            if (!wasFeedloadedFromCache)
            {
                try
                {
                    base.tootTimelineData = await ClientHelper.Client.GetPublicTimeline(local: true);
                    nextPageMaxId = tootTimelineData.NextPageMaxId;
                    previousPageMinId = tootTimelineData.PreviousPageMinId;
                    previousPageSinceId = tootTimelineData.PreviousPageSinceId;

                    TootTimelineCollection = new ObservableCollection<Mastonet.Entities.Status>(tootTimelineData);
                }
                catch (Exception)
                {

                    await ErrorService.ShowConnectionError();
                }
            }
        }
    }
}

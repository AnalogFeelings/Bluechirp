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
    class HomeViewModel : TimelineViewModelBase
    {
        public override string ViewTitle { get; protected set; } = "Home";

        public HomeViewModel() : base()
        {

        }

        public override event EventHandler TootsAdded;
        public override event EventHandler<Status> StatusMarkerAdded;

        internal async override Task LoadFeedAsync()
        {
            bool wasFeedloadedFromCache = await AttemptToLoadFromCache();
            if (!wasFeedloadedFromCache)
            {
                try
                {
                    tootTimelineData = await ClientHelper.Client.GetHomeTimeline();
                    nextPageMaxId = tootTimelineData.NextPageMaxId;
                    previousPageMinId = tootTimelineData.PreviousPageMinId;
                    previousPageSinceId = tootTimelineData.PreviousPageSinceId;

                    TootTimelineCollection = new ObservableCollection<Status>(tootTimelineData);
                }
                catch (Exception)
                {
                    await ErrorService.ShowConnectionError();
                }
            }
        }

        internal async override Task AddOlderContentToFeed()
        {
            try
            {
                var olderContent = await ClientHelper.Client.GetHomeTimeline(nextPageMaxId);
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

        internal async override Task AddNewerContentToFeed()
        {
            Mastonet.ArrayOptions options = new Mastonet.ArrayOptions();

            options.MinId = previousPageMinId;
            options.SinceId = previousPageSinceId;

            try
            {
                var newerContent = await ClientHelper.Client.GetHomeTimeline(options);

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
            catch
            {
                await ErrorService.ShowConnectionError();
            }

            TootsAdded?.Invoke(null, EventArgs.Empty);
        }



        internal async override Task CacheTimeline(Status currentTopVisibleStatus)
        {
            var timelineSettings = new TimelineSettings(nextPageMaxId, previousPageMinId, previousPageSinceId, TimelineType.Home);
            await CacheService.CacheTimeline(tootTimelineData, currentTopVisibleStatus, timelineSettings);

        }

        protected async override Task<bool> AttemptToLoadFromCache()
        {
            bool cacheWasLoaded = false;
            var cacheLoadResult = await CacheService.LoadTimelineCache(TimelineType.Home);
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
    }
}

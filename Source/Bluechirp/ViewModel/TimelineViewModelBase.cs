using Mastonet;
using Mastonet.Entities;
using MastoParserLib.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bluechirp.Services;
using Bluechirp.Library.Commands;
using Bluechirp.Library.Enums;
using Bluechirp.Library.Helpers;
using Bluechirp.Library.Model;
using Bluechirp.Library.Services;

namespace Bluechirp.ViewModel
{
    public abstract class TimelineViewModelBase : Library.Model.Notifier
    {
        protected string previousPageSinceId;
        protected string previousPageMinId;
        protected string nextPageMaxId;
        protected MastodonList<Status> tootTimelineData;
        public RelayCommandWithParameter DeleteCommand;

        protected abstract TimelineType timelineType { get; set; }
        public abstract string ViewTitle { get; protected set; }

        public event EventHandler TootsAdded;
        public event EventHandler<Status> StatusMarkerAdded;

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

        protected abstract Task<MastodonList<Status>> GetNewerTimeline(ArrayOptions options);
        protected abstract Task<MastodonList<Status>> GetOlderTimeline();
        protected abstract Task<MastodonList<Status>> GetTimeline();

        public TimelineViewModelBase()
        {
            DeleteCommand = new RelayCommandWithParameter(DeleteToot);
        }


        protected async Task<bool> AttemptToLoadFromCache()
        {
            bool cacheWasLoaded = false;
            var (wasTimelineLoaded, cacheToReturn) = await CacheService.LoadTimelineCache(timelineType);
            if (wasTimelineLoaded)
            {
                var cache = cacheToReturn;
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


        internal async Task CacheTimeline(Status currentTopVisibleStatus)
        {
            var timelineSettings = new TimelineSettings(nextPageMaxId, previousPageMinId, previousPageSinceId, timelineType);
            await CacheService.CacheTimeline(tootTimelineData, currentTopVisibleStatus, timelineSettings);
        }

        internal async Task CacheTimelineToRuntime(Status currentTopVisibleStatus)
        {
            //var timelineSettings = 
        }

        private void DeleteToot(object obj)
        {
            if (obj is Status tootToDelete)
            {
                TootTimelineCollection.Remove(tootToDelete);
            }
        }

        internal async Task LoadFeedAsync()
        {
            bool wasFeedloadedFromCache = await AttemptToLoadFromCache();
            if (!wasFeedloadedFromCache)
            {
                try
                {
                    tootTimelineData = await GetTimeline();
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

        internal async Task AddOlderContentToFeed()
        {
            try
            {
                var olderContent = await GetOlderTimeline();
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

        internal async Task AddNewerContentToFeed()
        {
            ArrayOptions options = new ArrayOptions
            {
                MinId = previousPageMinId,
                SinceId = previousPageSinceId
            };

            try
            {
                var newerContent = await GetNewerTimeline(options);

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
    }
}

﻿using Mastonet;
using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Enums;
using Tooter.Helpers;
using Tooter.Model;
using Tooter.Services;

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
            await AttemptToLoadFromCache();
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

        protected override Task<bool> AttemptToLoadFromCache()
        {
            throw new NotImplementedException();
        }
    }
}

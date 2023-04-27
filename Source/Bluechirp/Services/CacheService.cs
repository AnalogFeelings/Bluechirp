using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BluechirpLib.Enums;
using BluechirpLib.Helpers;
using BluechirpLib.Model;
using BluechirpLib.Services;
using Windows.Storage;
using Bluechirp.View;

namespace Bluechirp.Services
{
    public class CacheService
    {
        static TimelineView currentTimeline;
        internal static string KeyboardShortuctsContent = "";

        internal async static Task CacheTimeline(MastodonList<Status> timeline, Status currentStatusMarker, TimelineSettings timelineSettings)
        {
            TimelineCache cachedTimeline = new TimelineCache(timeline, currentStatusMarker, timelineSettings);
            await ClientDataHelper.StoreTimelineCacheAsync(cachedTimeline);
        }

        internal static void SwapCurrentTimeline(TimelineView newTimeline)
        {
            currentTimeline = newTimeline;
        }

        internal async static Task CacheCurrentTimeline()
        {
            if (currentTimeline != null)
            {
                await currentTimeline.TryCacheTimeline();
            }
        }

        internal async static Task<(bool wasTimelineLoaded, TimelineCache cacheToReturn)> LoadTimelineCache(TimelineType timelineType)
        {
            var cacheLoadResult = await ClientDataHelper.LoadTimelineFromFileAsync(timelineType);
            bool wasTimelineLoaded = cacheLoadResult.wasTimelineLoaded;
            TimelineCache cacheToReturn = cacheLoadResult.cacheToReturn;

            // Loaded cache from a file is stored into runtime cache to prevent having to load from file repeatedly.
            // Also cache file has been deleted already by this point.
            //if (wasTimelineLoaded)
            //{
            //    RuntimeCacheService.StoreCache(cacheToReturn, cacheToReturn.CurrentTimelineSettings.CurrentTimelineType);
            //}
            return (wasTimelineLoaded, cacheToReturn);
        }

        internal async static Task LoadKeyboardShortcutsContent()
        {
            var shortcutsFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/FediShortcuts.txt"));
            KeyboardShortuctsContent = await FileIO.ReadTextAsync(shortcutsFile);
        }

        
    }
}
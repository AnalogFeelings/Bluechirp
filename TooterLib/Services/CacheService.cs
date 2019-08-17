using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Enums;
using Tooter.Helpers;
using Tooter.Model;
using Tooter.View;
using Windows.UI.Xaml.Controls;

namespace Tooter.Services
{
    public class CacheService
    {
        static Page currentTimeline;

        public static event EventHandler TimeToCacheAndStopListening;

        public static event EventHandler TimeToListen;

        internal async static Task CacheTimeline(MastodonList<Status> timeline, Status currentStatusMarker, TimelineSettings timelineSettings)
        {
            TimelineCache cachedTimeline = new TimelineCache(timeline, currentStatusMarker, timelineSettings);
            await ClientDataHelper.StoreTimelineCache(cachedTimeline);
        }

        internal static void SwapCurrentTimeline(Page newTimeline)
        {
            currentTimeline = newTimeline;
        }

        internal static void CacheCurrentTimeline()
        {
            TryCacheTimeline();
        }

        internal async static Task<(bool wasTimelineLoaded, TimelineCache cacheToReturn)> LoadTimelineCache(TimelineType timelineType)
        {
            var cacheLoadResult = await ClientDataHelper.LoadTimelineFromFileAsync(timelineType);
            return (cacheLoadResult.wasTimelineLoaded, cacheLoadResult.cacheToReturn);

        } 

        public static void TryCacheTimeline()
        {
            TimeToCacheAndStopListening?.Invoke(null, EventArgs.Empty);
        }
    }
}

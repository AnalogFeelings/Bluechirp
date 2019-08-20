using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooterLib.Enums;
using TooterLib.Helpers;
using TooterLib.Model;
using Tooter.View;

namespace Tooter.Services
{
    public class CacheService
    {
        static TimelineView currentTimeline;

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
            return (cacheLoadResult.wasTimelineLoaded, cacheLoadResult.cacheToReturn);

        }
    }
}
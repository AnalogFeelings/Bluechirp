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

namespace Tooter.Services
{
    public class CacheService
    {
        static TimelineView currentTimeline;

        internal async static Task CacheTimeline(MastodonList<Status> timeline, Status currentStatusMarker, TimelineSettings timelineSettings)
        {
            TimelineCache cachedTimeline = new TimelineCache(timeline, currentStatusMarker, timelineSettings);
            await ClientDataHelper.StoreTimelineCache(cachedTimeline);
        }

        internal static void SwapCurrentTimeline(TimelineView newTimeline)
        {
            currentTimeline = newTimeline;
        }

        internal async static Task CacheCurrentTimeline()
        {
            await currentTimeline.TryCacheTimeline();
        }
    }
}

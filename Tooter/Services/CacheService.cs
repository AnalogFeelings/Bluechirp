using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Enums;
using Tooter.Helpers;
using Tooter.Model;

namespace Tooter.Services
{
    public class CacheService
    {
        internal static event EventHandler TimelineCacheRequested;

        internal static void RequestTimelinesToBeCached()
        {
            TimelineCacheRequested?.Invoke(null, EventArgs.Empty);
        }

        internal static void CacheTimeline(MastodonList<Status> timeline, Status currentStatusMarker, TimelineType timelineType)
        {
            TimelineCache cachedTimeline = new TimelineCache(timeline, currentStatusMarker, timelineType);
            ClientDataHelper.StoreTimelineCache(cachedTimeline);
        }
    }
}

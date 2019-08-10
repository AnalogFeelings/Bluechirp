using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Enums;

namespace Tooter.Model
{
    public class TimelineCache
    {
        public MastodonList<Status> Toots;
        public Status CurrentStatusMarker;
        public TimelineType CacheTimelineType;

        public TimelineCache()
        {

        }

        public TimelineCache(MastodonList<Status> toots, Status currentStatusMarker, TimelineType timelineType)
        {
            Toots = toots;
            CurrentStatusMarker = currentStatusMarker;
            CacheTimelineType = timelineType;
        }
    }
}

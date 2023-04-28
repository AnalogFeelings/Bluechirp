using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bluechirp.Library.Enums;

namespace Bluechirp.Library.Model
{
    public class TimelineCache
    {
        public MastodonList<Status> Toots;
        public Status CurrentStatusMarker;
        public TimelineSettings CurrentTimelineSettings;

        public TimelineCache()
        {

        }

        public TimelineCache(MastodonList<Status> toots, Status currentStatusMarker, TimelineSettings timelineType)
        {
            Toots = toots;
            CurrentStatusMarker = currentStatusMarker;
            CurrentTimelineSettings = timelineType;
        }
    }
}

using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooterLib.Enums;
using TooterLib.Model;

namespace TooterTests.Generators
{
    internal class TimelineData
    {
        internal static TimelineCache GenerateTimelineCache(TimelineType timelineType)
        {
            MastodonList<Status> fakeTimeline = new MastodonList<Status>();
            fakeTimeline.AddRange(collection: new Status[] { new Status { Content = "hii" }, new Status { Content = "Hello" }, new Status { Content = "REEEE" } });
            TimelineCache cache = new TimelineCache(fakeTimeline, fakeTimeline[1], new TimelineSettings(0, 0, 0, timelineType));
            return cache;
        }
    }
}

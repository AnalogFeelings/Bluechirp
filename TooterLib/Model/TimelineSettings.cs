using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Enums;

namespace Tooter.Model
{
    public class TimelineSettings
    {
        public long? NextPageMaxID;
        public long? PreviousPageMinID;
        public long? PreviousPageSinceID;
        public TimelineType CurrentTimelineType;

        public TimelineSettings(long? nextPageMaxID, long? previousPageMinID, long? previousPageSinceID, TimelineType timelineType)
        {
            NextPageMaxID = nextPageMaxID;
            PreviousPageMinID = previousPageMinID;
            PreviousPageSinceID = previousPageSinceID;
            CurrentTimelineType = timelineType;
        }
    }
}

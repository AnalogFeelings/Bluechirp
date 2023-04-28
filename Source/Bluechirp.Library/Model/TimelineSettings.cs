using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bluechirp.Library.Enums;

namespace Bluechirp.Library.Model
{
    public class TimelineSettings
    {
        public string NextPageMaxID;
        public string PreviousPageMinID;
        public string PreviousPageSinceID;
        public TimelineType CurrentTimelineType;

        public TimelineSettings(string nextPageMaxID, string previousPageMinID, string previousPageSinceID, TimelineType timelineType)
        {
            NextPageMaxID = nextPageMaxID;
            PreviousPageMinID = previousPageMinID;
            PreviousPageSinceID = previousPageSinceID;
            CurrentTimelineType = timelineType;
        }
    }
}

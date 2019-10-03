using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.ViewModel;
using TooterLib.Model;

namespace Tooter.Model
{
    public class TimelineCacheV2
    {
        public TimelineViewModelBase ViewModel;
        public Status CurrentStatusMarker;
        public TimelineSettings CurrentTimelineSettings;

        public TimelineCacheV2()
        {

        }

        public TimelineCacheV2(TimelineViewModelBase viewModel, Status currentStatusMarker, TimelineSettings timelineType)
        {
            ViewModel = viewModel;
            CurrentStatusMarker = currentStatusMarker;
            CurrentTimelineSettings = timelineType;
        }
    }
}

using Bluechirp.Library.Enums;
using Mastonet;
using Mastonet.Entities;
using System;
using System.Threading.Tasks;

namespace Bluechirp.Library.ViewModel.Timelines
{
    public class HomeTimelineViewModel : BaseTimelineViewModel
    {
        public override string TimelineTitle { get; protected init; } = "Home Timeline";
        public override TimelineType TimelineType { get; protected init; } = TimelineType.Home;

        protected override Task<MastodonList<Status>> GetNewerTimeline(ArrayOptions options)
        {
            throw new NotImplementedException();
        }

        protected override Task<MastodonList<Status>> GetOlderTimeline()
        {
            throw new NotImplementedException();
        }

        protected override Task<MastodonList<Status>> GetTimeline()
        {
            throw new NotImplementedException();
        }
    }
}

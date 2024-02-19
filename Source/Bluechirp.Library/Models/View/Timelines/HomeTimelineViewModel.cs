using Bluechirp.Library.Enums;
using Bluechirp.Library.Services.Security;
using Mastonet;
using Mastonet.Entities;
using System.Threading.Tasks;

namespace Bluechirp.Library.Models.View.Timelines
{
    /// <summary>
    /// The view model for the Home timeline.
    /// </summary>
    public class HomeTimelineViewModel : BaseTimelineViewModel
    {
        public override string TimelineTitle { get; protected init; } = "Home Timeline";
        public override TimelineType TimelineType { get; protected init; } = TimelineType.Home;

        private IAuthService _authService;

        public HomeTimelineViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        protected override async Task<MastodonList<Status>> GetNewerTimeline(ArrayOptions options)
        {
            return await _authService.Client.GetHomeTimeline(options);
        }

        protected override async Task<MastodonList<Status>> GetOlderTimeline()
        {
            ArrayOptions options = new ArrayOptions()
            {
                MaxId = NextPageMaxId
            };

            return await _authService.Client.GetHomeTimeline(options);
        }

        protected override async Task<MastodonList<Status>> GetTimeline()
        {
            return await _authService.Client.GetHomeTimeline();
        }
    }
}

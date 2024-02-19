using Bluechirp.Library.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using Mastonet;
using Mastonet.Entities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Bluechirp.Library.Models.View.Timelines
{
    public abstract partial class BaseTimelineViewModel : ObservableObject
    {
        public abstract string TimelineTitle { get; protected init; }
        public abstract TimelineType TimelineType { get; protected init; }

        protected string PreviousPageSinceId;
        protected string PreviousPageMinId;
        protected string NextPageMaxId;
        protected MastodonList<Status> TootTimelineData;

        [ObservableProperty]
        private ObservableCollection<Status> _tootTimelineCollection;

        protected abstract Task<MastodonList<Status>> GetNewerTimeline(ArrayOptions options);
        protected abstract Task<MastodonList<Status>> GetOlderTimeline();
        protected abstract Task<MastodonList<Status>> GetTimeline();

        /// <summary>
        /// Loads the timeline feed.
        /// </summary>
        public async Task LoadFeedAsync()
        {
            TootTimelineData = await GetTimeline();
            NextPageMaxId = TootTimelineData.NextPageMaxId;
            PreviousPageMinId = TootTimelineData.PreviousPageMinId;
            PreviousPageSinceId = TootTimelineData.PreviousPageSinceId;

            TootTimelineCollection = new ObservableCollection<Status>(TootTimelineData);
        }
    }
}

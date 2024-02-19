using Bluechirp.Library.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using Mastonet;
using Mastonet.Entities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Bluechirp.Library.ViewModel.Timelines
{
    public abstract partial class BaseTimelineViewModel : ObservableObject
    {
        public abstract string TimelineTitle { get; protected init; }
        public abstract TimelineType TimelineType { get; protected init; }

        [ObservableProperty]
        private ObservableCollection<Status> _tootTimelineCollection;

        protected abstract Task<MastodonList<Status>> GetNewerTimeline(ArrayOptions options);
        protected abstract Task<MastodonList<Status>> GetOlderTimeline();
        protected abstract Task<MastodonList<Status>> GetTimeline();
    }
}

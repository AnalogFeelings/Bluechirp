using Bluechirp.Library.Enums;
using Bluechirp.Library.Models.View.Timelines;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Bluechirp.Views.Navigation
{
    /// <summary>
    /// Generic page for timeline views.
    /// </summary>
    public sealed partial class TimelinePage : Page
    {
        public BaseTimelineViewModel ViewModel => (BaseTimelineViewModel)this.DataContext;

        public TimelinePage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            TimelineType timelineType = (TimelineType)e.Parameter;
            BaseTimelineViewModel viewModel;

            switch(timelineType)
            {
                default:
                case TimelineType.Home:
                    viewModel = App.ServiceProvider.GetRequiredService<HomeTimelineViewModel>();
                    break;
            }

            this.DataContext = viewModel;

            await ViewModel.LoadFeedAsync();
        }
    }
}

#region License Information (GPLv3)
// Bluechirp - A modern, native client for the Mastodon social media.
// Copyright (C) 2023 Analog Feelings and contributors.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
#endregion

using Bluechirp.Library.Enums;
using Bluechirp.Library.Models.View.Timelines;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Bluechirp.Views.Navigation;

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
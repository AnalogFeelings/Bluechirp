#region License Information (GPLv3)
// Bluechirp - A modern, native client for the Mastodon social media.
// Copyright (C) 2023-2024 Analog Feelings and contributors.
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
using CommunityToolkit.Mvvm.ComponentModel;
using Mastonet;
using Mastonet.Entities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Bluechirp.Library.Models.View.Timelines;

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
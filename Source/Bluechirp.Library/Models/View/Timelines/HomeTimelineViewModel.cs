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
using Bluechirp.Library.Services.Security;
using Mastonet;
using Mastonet.Entities;
using System.Threading.Tasks;

namespace Bluechirp.Library.Models.View.Timelines;

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
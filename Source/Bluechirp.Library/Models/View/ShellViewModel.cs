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

using Bluechirp.Library.Helpers;
using Bluechirp.Library.Services.Security;
using CommunityToolkit.Mvvm.ComponentModel;
using Mastonet.Entities;

namespace Bluechirp.Library.Models.View;

/// <summary>
/// Implements a view model for a shell page.
/// </summary>
public partial class ShellViewModel : ObservableObject
{
    [ObservableProperty]
    private Account _currentAccount;

    private IAuthService _authService;

    public string TitleBadgeText
    {
        get
        {
#if DEBUG
            return "DEV";
#else
                return string.Empty;
#endif
        }
    }

    public ShellViewModel(IAuthService authService)
    {
        _authService = authService;

        _currentAccount = AsyncHelper.RunSync(() => _authService.Client.GetCurrentUser());
    }
}
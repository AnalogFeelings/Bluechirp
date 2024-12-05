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

using Bluechirp.Library.Constants;
using Bluechirp.Library.Services.Environment;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;

namespace Bluechirp.Library.Models.View.Navigation;

/// <summary>
/// Implements a view model for the settings page.
/// </summary>
public partial class SettingsViewModel
{
    public string AppVersion => _infoService.AppVersion.ToString();

    private Uri _feedbackUri = new Uri(AppConstants.FEEDBACK_URI);
    private Uri _bugUri = new Uri(AppConstants.BUG_URI);

    private IInfoService _infoService;

    public SettingsViewModel(IInfoService infoService)
    {
        _infoService = infoService;
    }

    [RelayCommand]
    private async Task OpenFeedbackPage()
    {
        await Launcher.LaunchUriAsync(_feedbackUri);
    }

    [RelayCommand]
    private async Task OpenBugReportPage()
    {
        await Launcher.LaunchUriAsync(_bugUri);
    }

    [RelayCommand]
    private async Task OpenLogFile()
    {
        StorageFolder folder = await ApplicationData.Current.LocalFolder.GetFolderAsync(AppConstants.LOG_FOLDER);

        await Launcher.LaunchFolderAsync(folder);
    }
}

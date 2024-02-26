﻿using Bluechirp.Library.Constants;
using Bluechirp.Library.Services.Environment;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using Windows.System;

namespace Bluechirp.Library.Models.View.Navigation;

/// <summary>
/// Implements a view model for the settings page.
/// </summary>
public partial class SettingsViewModel
{
    public string AppVersion => _infoService.AppVersion.ToString();

    private Uri _feedbackUri = new Uri(ApiConstants.FEEDBACK_URI);
    private Uri _bugUri = new Uri(ApiConstants.BUG_URI);

    private IInfoService _infoService;

    public SettingsViewModel(IInfoService infoService)
    {
        _infoService = infoService;
    }

    [RelayCommand]
    public async Task OpenFeedbackPage()
    {
        await Launcher.LaunchUriAsync(_feedbackUri);
    }

    [RelayCommand]
    public async Task OpenBugReportPage()
    {
        await Launcher.LaunchUriAsync(_bugUri);
    }
}
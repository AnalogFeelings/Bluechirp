using Bluechirp.Library.Services.Environment;

namespace Bluechirp.Library.Models.View.Navigation;

/// <summary>
/// Implements a view model for the settings page.
/// </summary>
public class SettingsViewModel
{
    public string AppVersion => _infoService.AppVersion.ToString();

    private IInfoService _infoService;

    public SettingsViewModel(IInfoService infoService)
    {
        _infoService = infoService;
    }
}

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
using Bluechirp.Library.Helpers;
using Bluechirp.Library.Services.Security;
using CommunityToolkit.Mvvm.ComponentModel;
using Mastonet.Entities;

namespace Bluechirp.Library.ViewModel
{
    /// <summary>
    /// Implements a view model for a shell page.
    /// </summary>
    public partial class ShellViewModel : ObservableObject
    {
        [ObservableProperty]
        private Account _currentAccount;

        private IAuthService _authService;

        public bool ShowDevBadge
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        public ShellViewModel(IAuthService authService)
        {
            _authService = authService;

            _currentAccount = AsyncHelper.RunSync(async () => await _authService.Client.GetCurrentUser()) ;
        }
    }
}

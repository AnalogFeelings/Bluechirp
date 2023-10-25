namespace Bluechirp.Library.ViewModel
{
    /// <summary>
    /// Implements a view model for a shell page.
    /// </summary>
    public partial class ShellViewModel
    {
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
    }
}

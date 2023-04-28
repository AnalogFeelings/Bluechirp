using Bluechirp.Library.Core;

namespace Bluechirp.Core
{
    /// <summary>
    /// Class that contains API constants to be used by the library.
    /// </summary>
    public static class ApiConstants
    {
        private const string _APP_NAME = "Bluechirp";
        private const string _APP_WEBSITE = "https://github.com/AestheticalZ/Bluechirp";
        private const string _REDIRECT_URI = "aestheticalz-bluechirp://mycallback";

        /// <summary>
        /// Gives the library all the required constants to function.
        /// </summary>
        internal static void SetApiConstants()
        {
            APIConstants.SetConstants(_APP_NAME, _APP_WEBSITE, _REDIRECT_URI);
        }
    }
}
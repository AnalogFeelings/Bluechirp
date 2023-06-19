using System;

namespace Bluechirp.Library.Model.Navigation
{
    /// <summary>
	/// Represents a page in a <see cref="NavigationView"/> control.
	/// </summary>
	public class NavigationPage
    {
        /// <summary>
        /// The tag of the page.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// The text shown in the content frame of the <see cref="NavigationView"/> control.
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Toggles the header text on or off.
        /// </summary>
        public bool ShowHeader { get; set; }

        /// <summary>
        /// The actual page to navigate to.
        /// </summary>
        public Type Page { get; set; }

        /// <summary>
        /// The view model to be used for the page.
        /// </summary>
        /// <remarks>
        /// Only set this when dealing with timelines. TODO: Replace this system.
        /// </remarks>
        public Type ViewModel { get; set; } = null;
    }
}

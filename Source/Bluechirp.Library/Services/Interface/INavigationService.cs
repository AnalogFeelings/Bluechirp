using Bluechirp.Library.Enums;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;

namespace Bluechirp.Library.Services.Interface
{
    /// <summary>
    /// Defines a service interface for handling content frame
    /// navigation.
    /// </summary>
    public interface INavigationService
    {
        /// <summary>
        /// The current content frame.
        /// </summary>
        public Frame TargetFrame { get; set; }

        /// <summary>
        /// The type of the content frame's page.
        /// </summary>
        public Type CurrentPageType { get; }

        /// <summary>
        /// Navigates back, if possible.
        /// </summary>
        public void GoBack();

        /// <summary>
        /// Navigates forward, if possible.
        /// </summary>
        public void GoForward();

        /// <summary>
        /// Navigates to the target page type.
        /// </summary>
        /// <param name="sourcePageType">The target page's type.</param>
        /// <returns><see langword="true"/> if the navigation was successful.</returns>
        public bool Navigate(PageType sourcePageType);

        /// <summary>
        /// Navigates to the target page type with a parameter.
        /// </summary>
        /// <param name="sourcePageType">The target page's type.</param>
        /// <param name="parameter">The parameter to pass to the target page.</param>
        /// <returns><see langword="true"/> if the navigation was successful.</returns>
        public bool Navigate(PageType sourcePageType, object parameter);

        /// <summary>
        /// Navigates to the target page type with a parameter and a custom
        /// transition override.
        /// </summary>
        /// <param name="sourcePageType">The target page's type.</param>
        /// <param name="parameter">The parameter to pass to the target page.</param>
        /// <param name="infoOverride">The transition override.</param>
        /// <returns><see langword="true"/> if the navigation was successful.</returns>
        public bool Navigate(PageType sourcePageType, object parameter, NavigationTransitionInfo infoOverride);
    }
}

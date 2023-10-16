using Bluechirp.Library.Enums;
using Bluechirp.Library.Services.Interface;
using Bluechirp.Views;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;

namespace Bluechirp.Services.Interface
{
    /// <summary>
    /// Implements a navigation helper service.
    /// </summary>
    internal class NavigationService : INavigationService
    {
        /// <inheritdoc/>
        public Frame TargetFrame { get; set; }

        /// <inheritdoc/>
        public Type CurrentPageType => TargetFrame.SourcePageType;

        /// <inheritdoc/>
        public void GoBack()
        {
            if(TargetFrame.CanGoBack)
                TargetFrame.GoBack();
        }

        /// <inheritdoc/>
        public void GoForward()
        {
            if (TargetFrame.CanGoForward)
                TargetFrame.GoForward();
        }

        /// <inheritdoc/>
        public bool Navigate(PageType sourcePageType)
        {
            Type targetType = sourcePageType switch
            {
                PageType.Login => typeof(LoginPage),
                PageType.Shell => typeof(ShellPage),
                _ => throw new ArgumentOutOfRangeException("Attempted to navigate to non-existant page.")
            };

            return TargetFrame.Navigate(targetType);
        }

        /// <inheritdoc/>
        public bool Navigate(PageType sourcePageType, object parameter)
        {
            Type targetType = sourcePageType switch
            {
                PageType.Login => typeof(LoginPage),
                PageType.Shell => typeof(ShellPage),
                _ => throw new ArgumentOutOfRangeException("Attempted to navigate to non-existant page.")
            };

            return TargetFrame.Navigate(targetType, parameter);
        }

        /// <inheritdoc/>
        public bool Navigate(PageType sourcePageType, object parameter, NavigationTransitionInfo infoOverride)
        {
            Type targetType = sourcePageType switch
            {
                PageType.Login => typeof(LoginPage),
                PageType.Shell => typeof(ShellPage),
                _ => throw new ArgumentOutOfRangeException("Attempted to navigate to non-existant page.")
            };

            return TargetFrame.Navigate(targetType, parameter, infoOverride);
        }
    }
}

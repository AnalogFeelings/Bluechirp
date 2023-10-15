using Bluechirp.Library.Services.Interface;
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
        public bool Navigate(Type sourcePageType)
        {
            return TargetFrame.Navigate(sourcePageType);
        }

        /// <inheritdoc/>
        public bool Navigate(Type sourcePageType, object parameter)
        {
            return TargetFrame.Navigate(sourcePageType, parameter);
        }

        /// <inheritdoc/>
        public bool Navigate(Type sourcePageType, object parameter, NavigationTransitionInfo infoOverride)
        {
            return TargetFrame.Navigate(sourcePageType, parameter, infoOverride);
        }
    }
}

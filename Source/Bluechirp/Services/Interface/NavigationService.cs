using Bluechirp.Library.Enums;
using Bluechirp.Library.Services.Interface;
using Bluechirp.Views;
using Bluechirp.Views.Navigation;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;

namespace Bluechirp.Services.Interface;

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
        if (TargetFrame.CanGoBack)
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
        Type targetType = AgnosticPageToAppPage(sourcePageType);

        return TargetFrame.Navigate(targetType);
    }

    /// <inheritdoc/>
    public bool Navigate(PageType sourcePageType, object parameter)
    {
        Type targetType = AgnosticPageToAppPage(sourcePageType);

        return TargetFrame.Navigate(targetType, parameter);
    }

    /// <inheritdoc/>
    public bool Navigate(PageType sourcePageType, object parameter, NavigationTransitionInfo infoOverride)
    {
        Type targetType = AgnosticPageToAppPage(sourcePageType);

        return TargetFrame.Navigate(targetType, parameter, infoOverride);
    }

    /// <summary>
    /// Converts a platform agnostic <see cref="PageType"/> to the app's native
    /// page type.
    /// </summary>
    /// <param name="pageType">The page type to convert.</param>
    /// <returns>The native version of the provided page type.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown if an invalid page is provided.
    /// </exception>
    private Type AgnosticPageToAppPage(PageType pageType)
    {
        return pageType switch
        {
            PageType.Login => typeof(LoginPage),
            PageType.Shell => typeof(ShellPage),
            PageType.Timeline => typeof(TimelinePage),
            _ => throw new ArgumentOutOfRangeException("Attempted to navigate to non-existant page.")
        };
    }
}
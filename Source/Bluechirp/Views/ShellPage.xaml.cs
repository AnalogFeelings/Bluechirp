#region License Information (GPLv3)
// Bluechirp - A modern, native client for the Mastodon social media.
// Copyright (C) 2023-2024 Analog Feelings and contributors.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
#endregion

using Bluechirp.Library.Enums;
using Bluechirp.Library.Models.View;
using Bluechirp.Library.Services.Interface;
using Bluechirp.Views.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Bluechirp.Views;

public sealed partial class ShellPage : Page
{
    private INavigationService _navigationService;

    public ShellViewModel ViewModel => (ShellViewModel)this.DataContext;

    public ShellPage()
    {
        this.InitializeComponent();
        this.DataContext = App.ServiceProvider.GetRequiredService<ShellViewModel>();

        _navigationService = App.ServiceProvider.GetRequiredService<INavigationService>();

        _navigationService.TargetFrame = ContentFrame;
        AppTitleBar.Window = MainWindow.Current;
    }

    private void NavigationViewControl_Loaded(object sender, RoutedEventArgs e)
    {
        NavigationViewControl.SelectedItem = NavigationViewControl.MenuItems[0];
    }

    private void AppTitleBar_BackButtonClick(object sender, RoutedEventArgs e)
    {
        if (_navigationService.TargetFrame.CanGoBack)
            _navigationService.TargetFrame.GoBack();
    }

    private void AppTitleBar_PaneButtonClick(object sender, RoutedEventArgs e)
    {
        NavigationViewControl.IsPaneOpen = !NavigationViewControl.IsPaneOpen;
    }

    private void NavigationViewControl_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        string selectedTag = args.SelectedItemContainer.Tag as string;

        switch(selectedTag)
        {
            case "home":
                if(DoTimelineChecks(TimelineType.Home))
                    return;

                _navigationService.Navigate(PageType.Timeline, TimelineType.Home, args.RecommendedNavigationTransitionInfo);

                break;
            case "posts":
                if(DoTimelineChecks(TimelineType.Featured))
                    return;

                _navigationService.Navigate(PageType.Timeline, TimelineType.Featured, args.RecommendedNavigationTransitionInfo);

                break;
            case "local":
                if(DoTimelineChecks(TimelineType.Local))
                    return;

                _navigationService.Navigate(PageType.Timeline, TimelineType.Local, args.RecommendedNavigationTransitionInfo);

                break;
            case "federated":
                if(DoTimelineChecks(TimelineType.Federated))
                    return;

                _navigationService.Navigate(PageType.Timeline, TimelineType.Federated, args.RecommendedNavigationTransitionInfo);

                break;
            case "favorites":
                if(DoTimelineChecks(TimelineType.Favorites))
                    return;

                _navigationService.Navigate(PageType.Timeline, TimelineType.Favorites, args.RecommendedNavigationTransitionInfo);

                break;
            case "bookmarks":
                if(DoTimelineChecks(TimelineType.Bookmarks))
                    return;

                _navigationService.Navigate(PageType.Timeline, TimelineType.Bookmarks, args.RecommendedNavigationTransitionInfo);

                break;
            default:
                return;
        }
    }

    /// <summary>
    /// Checks if the current page is the same timeline type as <paramref name="timelineType"/>
    /// </summary>
    /// <param name="timelineType">The timeline type to check against.</param>
    /// <returns><see langword="true"/> if the current timeline type matches <paramref name="timelineType"/>.</returns>
    private bool DoTimelineChecks(TimelineType timelineType)
    {
        if (_navigationService.CurrentPageType != typeof(TimelinePage))
            return false;

        TimelinePage timeline = _navigationService.TargetFrame.Content as TimelinePage;

        return timeline!.ViewModel.TimelineType == timelineType;
    }
}

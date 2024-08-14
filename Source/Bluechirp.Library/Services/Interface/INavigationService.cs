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
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;

namespace Bluechirp.Library.Services.Interface;

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
    public bool Navigate(PageType sourcePageType, object? parameter);

    /// <summary>
    /// Navigates to the target page type with a parameter and a custom
    /// transition override.
    /// </summary>
    /// <param name="sourcePageType">The target page's type.</param>
    /// <param name="parameter">The parameter to pass to the target page.</param>
    /// <param name="infoOverride">The transition override.</param>
    /// <returns><see langword="true"/> if the navigation was successful.</returns>
    public bool Navigate(PageType sourcePageType, object? parameter, NavigationTransitionInfo infoOverride);
}

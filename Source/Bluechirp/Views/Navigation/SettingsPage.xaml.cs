#region License Information (GPLv3)
// Bluechirp - A modern, native client for the Mastodon social media.
// Copyright (C) 2023 Analog Feelings and contributors.
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

using Bluechirp.Library.Models.View.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace Bluechirp.Views.Navigation;

/// <summary>
/// Contains application settings and information about
/// the application itself.
/// </summary>
public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel => (SettingsViewModel)this.DataContext;

    public SettingsPage()
    {
        this.InitializeComponent();
        this.DataContext = App.ServiceProvider.GetRequiredService<SettingsViewModel>();
    }
}

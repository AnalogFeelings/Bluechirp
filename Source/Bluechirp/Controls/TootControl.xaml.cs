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

using Bluechirp.Library.Models.View.Timelines;
using Mastonet.Entities;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Bluechirp.Controls;

public sealed partial class TootControl : UserControl
{
    public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(nameof(Status),
        typeof(Status), typeof(TootControl), null);

    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel),
        typeof(BaseTimelineViewModel), typeof(TootControl), null);

    public Status Status
    {
        get => (Status)GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }

    public BaseTimelineViewModel ViewModel
    {
        get => (BaseTimelineViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public TootControl()
    {
        this.InitializeComponent();
    }
}
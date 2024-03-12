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
using CommunityToolkit.Mvvm.ComponentModel;
using Mastonet.Entities;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Windows.Media.Core;

namespace Bluechirp.Controls;

[INotifyPropertyChanged]
public sealed partial class TootControl : UserControl
{
    public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(nameof(Status),
        typeof(Status), typeof(TootControl), new PropertyMetadata(null, StatusUpdated));

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

    public ObservableCollection<FrameworkElement> MediaItems { get; set; }

    public bool ShowMediaPips => MediaItems.Count > 1;

    public TootControl()
    {
        this.InitializeComponent();
        this.MediaItems = new ObservableCollection<FrameworkElement>();

        this.MediaItems.CollectionChanged += MediaItems_CollectionChanged;
    }

    private void MediaItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        OnPropertyChanged(nameof(ShowMediaPips));
    }

    private void UpdateMedia()
    {
        foreach(Attachment attachment in Status.MediaAttachments)
        {
            switch(attachment.Type)
            {
                case "video":
                    MediaPlayerElement videoPlayer = new MediaPlayerElement
                    {
                        PosterSource = new BitmapImage(new Uri(attachment.PreviewUrl)),
                        Source = MediaSource.CreateFromUri(new Uri(attachment.Url)),
                        AreTransportControlsEnabled = true,
                        TransportControls =
                        {
                            IsCompact = true
                        }
                    };

                    MediaItems.Add(videoPlayer);

                    break;
                case "gifv":
                    MediaPlayerElement gifPlayer = new MediaPlayerElement
                    {
                        Source = MediaSource.CreateFromUri(new Uri(attachment.Url)),
                        AreTransportControlsEnabled = false,
                        AutoPlay = true,
                        MediaPlayer =
                        {
                            IsLoopingEnabled = true,
                        }
                    };

                    MediaItems.Add(gifPlayer);

                    break;
                default:
                    BitmapImage bitmapImage = new BitmapImage(new Uri(attachment.Url));
                    Image imageControl = new Image
                    {
                        Source = bitmapImage,
                        HorizontalAlignment = HorizontalAlignment.Stretch
                    };

                    MediaItems.Add(imageControl);

                    break;
            }
        }
    }

    private static void StatusUpdated(DependencyObject @object,  DependencyPropertyChangedEventArgs e)
    {
        TootControl control = (@object as TootControl)!;

        control.UpdateMedia();
    }
}

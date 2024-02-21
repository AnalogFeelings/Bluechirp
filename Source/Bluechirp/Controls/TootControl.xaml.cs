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
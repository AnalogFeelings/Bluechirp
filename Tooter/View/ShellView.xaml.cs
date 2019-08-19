using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using TooterLib.Helpers;
using Tooter.Services;
using Tooter.ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using TooterLib.Services;
using Tooter.Model;
using Tooter.Enums;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Tooter.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShellView : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Frame _activeFrame;

        public Frame ActiveFrame
        {
            get { return _activeFrame; }
            set
            {
                _activeFrame = value;
                NotifyPropertyChanged();
            }
        }

        Frame _homeFrame = new Frame();
        Frame _localFrame = new Frame();
        Frame _federatedFrame = new Frame();





        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ShellView()
        {
            this.InitializeComponent();
        }


        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ActiveFrame = _homeFrame;
            ActiveFrame.Navigate(typeof(TimelineView), typeof(HomeViewModel));
            NavService.CreateInstance(ActiveFrame);
            SwapTimelineToCache();
            MenuListView.SelectedIndex = 0;
            await ViewModel.DoAsyncPrepartions();
        }

        private async void MenuListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            bool shouldSwapTimeline = false;
            if (e.ClickedItem is ShellMenuItem menuItem)
            {
                switch (menuItem.ItemType)
                {
                    case ShellMenuItemType.HomeTimeline:
                        if (ActiveFrame != _homeFrame)
                        {
                            shouldSwapTimeline = true;

                            await TryCacheTimeline();

                            ActiveFrame = _homeFrame;

                            if (!CheckIfFrameHasContent())
                            {
                                ActiveFrame.Navigate(typeof(TimelineView), typeof(HomeViewModel));
                            }
                        }
                        break;
                    case ShellMenuItemType.LocalTimeline:
                        if (ActiveFrame != _localFrame)
                        {
                            shouldSwapTimeline = true;

                            await TryCacheTimeline();

                            ActiveFrame = _localFrame;

                            if (!CheckIfFrameHasContent())
                            {
                                ActiveFrame.Navigate(typeof(TimelineView), typeof(LocalViewModel));

                            }
                        }
                        break;
                    case Enums.ShellMenuItemType.FederatedTimeline:
                        if (ActiveFrame != _federatedFrame)
                        {
                            shouldSwapTimeline = true;

                            await TryCacheTimeline();

                            ActiveFrame = _federatedFrame;

                            if (!CheckIfFrameHasContent())
                            {
                                _federatedFrame.Navigate(typeof(TimelineView), typeof(FederatedViewModel));
                            }
                        }
                        break;
                }


                if (shouldSwapTimeline)
                {
                    SwapTimelineToCache();
                }

            }
        }

        private async Task TryCacheTimeline()
        {
            await CacheService.CacheCurrentTimeline();
        }

        private void SwapTimelineToCache()
        {
            try
            {
                var timelineToCache = (TimelineView)ActiveFrame.Content;
                CacheService.SwapCurrentTimeline(timelineToCache);

            }
            catch
            {

            }
        }

        private bool CheckIfFrameHasContent()
        {
            return ActiveFrame.Content != null;
        }
    }
}

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
using Windows.UI.Core;

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
            _homeFrame.Navigated += LinkNavToBackButton;
            _localFrame.Navigated += LinkNavToBackButton;
            _federatedFrame.Navigated += LinkNavToBackButton;
            SystemNavigationManager.GetForCurrentView().BackRequested += ShellView_BackRequested;
        }

        private void ShellView_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (ActiveFrame.CanGoBack)
            {
                ActiveFrame.GoBack();
                e.Handled = true;
            }
        }

        private void LinkNavToBackButton(object sender, NavigationEventArgs e)
        {
            if (sender is Frame navFrame)
            {
                BackButton.IsEnabled = navFrame.CanGoBack;
            }
        }



        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ActiveFrame = _homeFrame;
            ActiveFrame.Navigate(typeof(TimelineView), typeof(HomeViewModel));
            NavService.CreateInstance(ActiveFrame);
            SwapTimeline();
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
                        else
                        {
                            ScrollToTop(ActiveFrame);
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
                        else
                        {
                            ScrollToTop(ActiveFrame);
                        }
                        break;
                    case ShellMenuItemType.FederatedTimeline:
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
                        else
                        {
                            ScrollToTop(ActiveFrame);
                        }
                        break;
                }


                if (shouldSwapTimeline)
                {
                    SwapTimeline();
                }

            }
        }

        private void ScrollToTop(Frame activeFrame)
        {
            TimelineView timelineToScrollUp = null;
            try
            {
                timelineToScrollUp = (TimelineView)ActiveFrame.Content;
                timelineToScrollUp.ScrollToTop();
            }
            catch (Exception)
            {

            }
        }

        private async Task TryCacheTimeline()
        {
            await CacheService.CacheCurrentTimeline();
        }

        private void SwapTimeline()
        {
            UpdateBackButtonEvents();
            TimelineView timelineToCache = null;
            try
            {
                timelineToCache = (TimelineView)ActiveFrame.Content;

            }
            catch
            {

            }
            CacheService.SwapCurrentTimeline(timelineToCache);
        }

        private void UpdateBackButtonEvents()
        {
            BackButton.IsEnabled = ActiveFrame.CanGoBack;
        }

        private bool CheckIfFrameHasContent()
        {
            return ActiveFrame.Content != null;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (ActiveFrame.CanGoBack)
            {
                ActiveFrame.GoBack();
            }
        }
    }
}

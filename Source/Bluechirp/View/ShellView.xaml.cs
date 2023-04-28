using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Bluechirp.Library.Helpers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Bluechirp.Library.Services;
using Windows.UI.Core;
using Bluechirp.Dialogs;
using Bluechirp.Enums;
using Bluechirp.Model;
using Bluechirp.Services;
using Bluechirp.ViewModel;
using EmojiDebugSystem;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Bluechirp.View
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
            GlobalKeyboardShortcutService.GlobalShortcutPressed += GlobalKeyboardShortcutService_GlobalShortcutPressed;
        }

        private async void GlobalKeyboardShortcutService_GlobalShortcutPressed(object sender, ShortcutType e)
        {
            switch (e)
            {
                case ShortcutType.Home:
                    await DecideHowToSwap(ViewModel.MenuListItems[0]);
                    MenuListView.SelectedIndex = 0;
                    break;
                case ShortcutType.Local:
                    await DecideHowToSwap(ViewModel.MenuListItems[1]);
                    MenuListView.SelectedIndex = 1;
                    break;
                case ShortcutType.Federated:
                    await DecideHowToSwap(ViewModel.MenuListItems[2]);
                    MenuListView.SelectedIndex = 2;
                    break;
                case ShortcutType.Help:
                    // TODO: Show Keyboard shortcuts dialog
                    //EmojiDebug.WriteLine(DebugEmoji.Celebrate, "Keyboard Shortcuts Help Key Combo!");
                    var kbShortuctsDialog = new KeyboardShortcutsDialog();
                    try
                    {
                        await kbShortuctsDialog.ShowAsync();
                    }
                    catch (Exception)
                    {

                        
                    }
                    break;
                default:
                    break;
            }
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

            if (e.ClickedItem is ShellMenuItem menuItem)
            {
                await DecideHowToSwap(menuItem);
            }
        }

        private async Task DecideHowToSwap(ShellMenuItem menuItem)
        {
            bool shouldSwapTimeline = false;
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

        private void MenuListView_Loaded(object sender, RoutedEventArgs e)
        {
            if (ActiveFrame == _homeFrame)
            {
                MenuListView.SelectedIndex = 0;
            }
            else if (ActiveFrame == _localFrame)
            {
                MenuListView.SelectedIndex = 1;
            }
            else if (ActiveFrame == _federatedFrame)
            {
                MenuListView.SelectedIndex = 2;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Tooter.Helpers;
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
            NavService.CreateInstance(ActiveFrame);
            ActiveFrame.Navigate(typeof(TimelineView), typeof(HomeViewModel));
            await ViewModel.DoAsyncPrepartions();
        }

        private void MenuListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is FontIcon menuListItem)
            {
                if (menuListItem == HomeButtonIcon && !HomeButton.IsSelected)
                {
                    ActiveFrame = _homeFrame;

                    if (!CheckIfFrameHasContent())
                    {
                        ActiveFrame.Navigate(typeof(TimelineView), typeof(HomeViewModel));
                    }
                }
                else if (menuListItem == LocalButtonIcon && !LocalButton.IsSelected)
                {
                    ActiveFrame = _localFrame;
                    if (!CheckIfFrameHasContent())
                    {
                        ActiveFrame.Navigate(typeof(TimelineView), typeof(LocalViewModel)); 
                    }
                }
                else if (menuListItem == FederatedButtonIcon && !FederatedButton.IsSelected)
                {
                    ActiveFrame = _federatedFrame;
                    if (!CheckIfFrameHasContent())
                    {
                        _federatedFrame.Navigate(typeof(TimelineView), typeof(FederatedViewModel)); 
                    }
                }
            }

        }


        private bool CheckIfFrameHasContent()
        {
            return ActiveFrame.Content != null;
        }
    }
}

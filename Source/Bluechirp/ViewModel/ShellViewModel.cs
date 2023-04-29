using Bluechirp.Dialogs;
using Bluechirp.Enums;
using Bluechirp.Library.Commands;
using Bluechirp.Library.Helpers;
using Bluechirp.Library.Services;
using Bluechirp.Model;
using Bluechirp.Services;
using Bluechirp.View;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bluechirp.ViewModel
{
    internal partial class ShellViewModel : ObservableObject
    {
        [ObservableProperty]
        private Account _currentUser;

        public List<ShellMenuItem> MenuListItems { get; set; } = new List<ShellMenuItem>();

        public ShellViewModel()
        {
            App.Current.EnteredBackground += Current_EnteredBackground;
            CreateMenuListItems();
        }

        internal async Task DoAsyncPrepartions()
        {
            try
            {
                CurrentUser = await ClientHelper.Client.GetCurrentUser();
            }
            catch (Exception)
            {
                await ErrorService.ShowConnectionError();
            }
        }

        internal async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            await ClientHelper.MakeLogoutPreprationsAsync();
            NavService.CreateInstance((Frame)Window.Current.Content);
            NavService.Instance.Navigate(typeof(LoginView));
        }

        private void CreateMenuListItems()
        {
            ShellMenuItem[] menuItems = new ShellMenuItem[]
            {
                new ShellMenuItem(ShellMenuItemType.HomeTimeline, "\xE80F"),
                new ShellMenuItem(ShellMenuItemType.LocalTimeline, "\xE716"),
                new ShellMenuItem(ShellMenuItemType.FederatedTimeline, "\xE909"),
            };

            MenuListItems.AddRange(menuItems);
        }

        private async void Current_EnteredBackground(object sender, Windows.ApplicationModel.EnteredBackgroundEventArgs e)
        {
            var deferral = e.GetDeferral();
            await CacheService.CacheCurrentTimeline();
            deferral.Complete();
        }

        [RelayCommand]
        private async Task OpenNewTootDialog()
        {
            await new NewTootDialog(CurrentUser.StaticAvatarUrl).ShowAsync();
        }
    }
}

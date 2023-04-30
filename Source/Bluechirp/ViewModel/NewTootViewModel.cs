using Bluechirp.Library.Commands;
using Bluechirp.Library.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bluechirp.ViewModel
{
    partial class NewTootViewModel : ObservableObject
    {
        private const int MastodonMaxStatusCharacters = 500;

        [ObservableProperty]
        private bool _isTootButtonEnabled;

        [ObservableProperty]
        private bool _isStatusEmpty;

        [ObservableProperty]
        private bool _hasReachedCharLimit;

        [ObservableProperty]
        private string _charCountString;

        [ObservableProperty]
        private string statusContent;

        [ObservableProperty]
        private Mastonet.Visibility _statusVisibilty;

        internal NewTootViewModel()
        {
            UpdateCharCountString(0);
        }

        partial void OnIsStatusEmptyChanged(bool value)
        {
            CheckIfTootButtonShouldBeEnabled();
        }

        partial void OnHasReachedCharLimitChanged(bool value)
        {
            CheckIfTootButtonShouldBeEnabled();
        }

        private void CheckIfTootButtonShouldBeEnabled()
        {
            IsTootButtonEnabled = !HasReachedCharLimit && !IsStatusEmpty;
        }

        [RelayCommand]
        protected async virtual Task SendTootAsync()
        {
            try
            {
                await ClientHelper.Client.PublishStatus(StatusContent, StatusVisibilty);
            }
            catch (Exception)
            {

            }
        }

        internal void StatusContentChanged(object sender, TextChangedEventArgs e)
        {
            var currentTextBox = (TextBox)sender;
            var charCountResult = CharCounterHelper.CountCharactersWithLimit(currentTextBox.Text, MastodonMaxStatusCharacters);
            UpdateCharCountString(charCountResult.charactersFound);

            if (charCountResult.characterLimitExceeded != HasReachedCharLimit)
            {
                HasReachedCharLimit = charCountResult.characterLimitExceeded;
            }
        }

        private void UpdateCharCountString(int charactersFound)
        {
            CharCountString = $"{charactersFound}/{MastodonMaxStatusCharacters} Characters";
            IsStatusEmpty = charactersFound > 0 == false;
        }

        internal void VisibilityOptionSelected(object sender, RoutedEventArgs e)
        {
            var selectedRadio = sender as RadioMenuFlyoutItem;
            StatusVisibilty = ConvertTagToPostVisibility(selectedRadio.Tag);
        }


        private Mastonet.Visibility ConvertTagToPostVisibility(object tag)
        {
            var tagValue = (string)tag;
            int tagAsNum = int.Parse(tagValue);
            return (Mastonet.Visibility)tagAsNum;

        }
    }
}

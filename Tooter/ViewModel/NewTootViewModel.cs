using Mastonet.Entities;
using MastoParserLib.Model;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooterLib.Commands;
using TooterLib.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Tooter.ViewModel
{
    class NewTootViewModel : Notifier
    {

        public RelayCommand SendTootCommand;

        private bool _isTootButtonEnabled;

        public bool IsTootButtonEnabled
        {
            get { return _isTootButtonEnabled; }
            set
            {
                _isTootButtonEnabled = value;
                NotifyPropertyChanged();
            }
        }


        private bool _isStatusEmpty;

        private bool IsStatusEmpty
        {
            get { return _isStatusEmpty; }
            set
            {
                _isStatusEmpty = value;
                CheckIfTootButtonShouldBeEnabled();
            }
        }

        

        private bool _hasReachedCharLimit;

        public bool HasReachedCharLimit
        {
            get { return _hasReachedCharLimit; }
            set
            {
                _hasReachedCharLimit = value;
                NotifyPropertyChanged();
                CheckIfTootButtonShouldBeEnabled();
            }
        }


        const int MastodonMaxStatusCharacters = 500;
        private string _charCountString;

        public string CharCountString
        {
            get { return _charCountString; }
            set
            {
                _charCountString = value;
                NotifyPropertyChanged();
            }
        }


        private string statusContent;

        public string StatusContent
        {
            get { return statusContent; }
            set
            {
                statusContent = value;
                NotifyPropertyChanged();
            }
        }


        private Mastonet.Visibility _statusVisibilty;

        public Mastonet.Visibility StatusVisibilty
        {
            get { return _statusVisibilty; }
            set
            {
                _statusVisibilty = value;
                NotifyPropertyChanged();
            }
        }


        internal NewTootViewModel()
        {
            SendTootCommand = new RelayCommand(async () => await SendNewToot());
            UpdateCharCountString(0);
        }


        private void CheckIfTootButtonShouldBeEnabled()
        {
            IsTootButtonEnabled = !HasReachedCharLimit && !IsStatusEmpty;
        }

        private async Task SendNewToot()
        {

            try
            {
                await ClientHelper.Client.PostStatus(StatusContent, StatusVisibilty);
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

            if (charCountResult.characterLimitReached != HasReachedCharLimit)
            {
                HasReachedCharLimit = charCountResult.characterLimitReached;
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

using Mastonet.Entities;
using MastoParserLib.Model;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Tooter.ViewModel
{
    class NewTootViewModel : Notifier
    {

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


        private Mastonet.Visibility _statustVisibilty;

        public Mastonet.Visibility StatusVisibilty
        {
            get { return _statustVisibilty; }
            set
            {
                _statustVisibilty = value;
                NotifyPropertyChanged();
            }
        }


        internal async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            await ClientHelper.Client.PostStatus(StatusContent, StatusVisibilty);
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

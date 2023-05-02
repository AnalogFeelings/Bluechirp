using Mastonet.Entities;
using Bluechirp.Parser;
using Bluechirp.Parser.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Bluechirp.Library.Helpers;
using Bluechirp.Library.Services;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Bluechirp.Dialogs;
using Bluechirp.Helpers;
using Bluechirp.Model;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Bluechirp.LocalControls
{
    public sealed partial class ExpandedTootTemplate : UserControl
    {
        public ExpandedToot CurrentStatus { get { return this.DataContext as ExpandedToot; } }
        public ExpandedTootTemplate()
        {
            this.InitializeComponent();
            this.DataContextChanged += UpdateData;
            this.KeyUp += TootTemplate_KeyUp;
        }

        private void TootTemplate_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            switch (e.Key)
            {
                case Windows.System.VirtualKey.R:
                    ReplyButton_Click(sender, null);
                    break;
                case Windows.System.VirtualKey.F:
                    FavouriteButton.IsChecked = !FavouriteButton.IsChecked;
                    FavouriteButton_Click(sender, null);
                    break;
                case Windows.System.VirtualKey.B:
                    ReblogButton.IsChecked = !ReblogButton.IsChecked;
                    ReblogButton_Click(sender, null);
                    break;
                default:
                    break;


            }
        }

        private void UpdateData(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            Status updatedStatus = (Status)args.NewValue;

            if (updatedStatus != null)
            {
                StatusContent.Blocks.Clear();

                Paragraph rootParagraph = new Paragraph();
                StatusContent.Blocks.Add(rootParagraph);

                MParser parser = new MParser();

                if (updatedStatus.Reblog != null)
                {
                    var reblogStatus = updatedStatus.Reblog;
                    if (reblogStatus.Content != null)
                    {
                        // On top of content, show that it's a reblog with indicators and stuff e.g (who reblogged)
                        // on top of original status

                        UpdateRebloggedByButton(updatedStatus.Account, reblogStatus.Account, true);
                        UpdateTimestamp(reblogStatus.CreatedAt);
                        try
                        {
                            List<MastoContent> parsedContent = parser.ParseContent(reblogStatus.Content);
                            TryDisplayParsedContent(parsedContent, reblogStatus);
                        }
                        catch
                        {
                            Run run = new Run { Text = $"ERROR!: {reblogStatus.Content}" };
                            run.Foreground = new SolidColorBrush(Colors.Red);
                            rootParagraph.Inlines.Add(run);
                        }
                        UpdateTootActions(reblogStatus);
                        AddMediaToStatus(reblogStatus.MediaAttachments.ToList());
                        UpdateTootMetadata(reblogStatus);
                    }
                }


                // Display regular status
                else
                {
                    UpdateRebloggedByButton(updatedStatus.Account, null);
                    if (updatedStatus.Content != null)
                    {
                        UpdateTimestamp(updatedStatus.CreatedAt);
                        try
                        {
                            List<MastoContent> parsedContent = parser.ParseContent(updatedStatus.Content);
                            TryDisplayParsedContent(parsedContent, updatedStatus);
                        }
                        catch
                        {
                            Run run = new Run { Text = $"ERROR!: {updatedStatus.Content}" };
                            run.Foreground = new SolidColorBrush(Colors.Red);
                            rootParagraph.Inlines.Add(run);
                        }
                        UpdateTootActions(updatedStatus);
                        AddMediaToStatus(updatedStatus.MediaAttachments.ToList());
                        UpdateTootMetadata(updatedStatus);
                    }

                }
            }

            args.Handled = true;
        }

        private void UpdateTootMetadata(Status status)
        {
            FavouritesCountTextBlock.Text = $"{status.FavouritesCount} Favourites";
            BoostsCountTextBlock.Text = $"{status.ReblogCount} Boosts";

            var formatter = new Windows.Globalization.DateTimeFormatting.DateTimeFormatter(" shortdate shorttime  ");
            string formattedDate = formatter.Format(new DateTimeOffset(status.CreatedAt));
            TootPostDateTextBlock.Text = formattedDate;

            TootVisibilityTextBlock.Text = status.Visibility.ToString();
            TootVisibilityIcon.Glyph = TootVisibilityHelper.TootVisibilityMap[status.Visibility];


            if (status.Application != null)
            {
                if (status.Application.Name != null)
                {
                    ApplicationLinkButton.Content = status.Application.Name;

                }
                if (status.Application.Website != null)
                {
                    ApplicationLinkButton.NavigateUri = new Uri(status.Application.Website);

                }
                ApplicationStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                ApplicationStackPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateTimestamp(DateTime createdAt)
        {
            StatusTimestamp.Text = TimestampHelper.FormatTimestamp(createdAt);
        }

        private void UpdateTootActions(Status status)
        {
            FavouriteButton.IsChecked = status.Favourited;
            ReblogButton.IsChecked = status.Reblogged;
        }

        private void TryDisplayParsedContent(List<MastoContent> parsedContent, Status status)
        {
            bool doesANewParagraphNeedToBeCreated = false;
            for (int i = 0; i < parsedContent.Count; i++)
            {
                var item = parsedContent[i];
                switch (item.ContentType)
                {
                    case MastoContentType.Mention:
                        List<Mention> mentions = (List<Mention>)status.Mentions;
                        TryAddMentions(mentions, item.Content);
                        break;
                    case MastoContentType.Link:
                        TryAddLinks(item.Content);
                        break;
                    case MastoContentType.Text:
                        var textItem = (MastoText)item;
                        TryAddText(textItem, i, ref doesANewParagraphNeedToBeCreated);
                        break;
                    case MastoContentType.Hashtag:
                        List<Tag> tags = (List<Tag>)status.Tags;
                        TryAddHashtags(tags, item.Content);
                        break;
                    default:
                        break;
                }

            }
        }

        private void UpdateRebloggedByButton(Account mainAccount, Account reblogAccount, bool isReblog = false)
        {
            if (isReblog)
            {
                UpdateNameTextBlocks(reblogAccount);
                UpdateAvatar(reblogAccount.AvatarUrl);
                RebloggedByButton.Visibility = Visibility.Visible;
                RebloggedByButton.Content = $"Reblogged by: {mainAccount.DisplayName}";
            }
            else
            {
                UpdateNameTextBlocks(mainAccount);
                UpdateAvatar(mainAccount.AvatarUrl);
                RebloggedByButton.Visibility = Visibility.Collapsed;
                RebloggedByButton.Content = "";
            }
        }

        private void TryAddHashtags(List<Tag> tags, string contentValue)
        {
            for (int tagIndex = 0; tagIndex < tags.Count; tagIndex++)
            {
                if (tags[tagIndex].Name == contentValue)
                {
                    Run tagRun = new Run { Text = $"#{contentValue}" };
                    Hyperlink hashtagLink = new Hyperlink();
                    hashtagLink.Inlines.Add(tagRun);
                    AddContentToTextBlock(hashtagLink);
                    break;
                }
            }
        }

        private void TryAddText(MastoText textItem, int loopsCompleted, ref bool doesANewParagraphNeedToBeCreated)
        {
            string contentToPrint = textItem.Content;
            if (loopsCompleted == 0)
            {
                contentToPrint = contentToPrint.TrimStart();
            }

            if (textItem.IsParagraph)
            {
                Run run = new Run { Text = $"{contentToPrint}" };
                AddContentToTextBlock(run);
                doesANewParagraphNeedToBeCreated = true;
            }
            else
            {
                Run run = new Run { Text = contentToPrint };
                AddContentToTextBlock(run, doesANewParagraphNeedToBeCreated);
                doesANewParagraphNeedToBeCreated = false;
            }
        }

        private void TryAddLinks(string contentValue)
        {
            Run linkRun = new Run { Text = contentValue };
            Hyperlink link = new Hyperlink
            {
                NavigateUri = new Uri(contentValue)
            };
            link.Inlines.Add(linkRun);
            AddContentToTextBlock(link);
        }

        private void TryAddMentions(List<Mention> mentions, string contentValue)
        {
            for (int mentionIndex = 0; mentionIndex < mentions.Count; mentionIndex++)
            {
                if (mentions[mentionIndex].UserName == contentValue)
                {
                    Run tagRun = new Run { Text = $"@{contentValue}" };
                    Hyperlink mentionLink = new Hyperlink();
                    mentionLink.Inlines.Add(tagRun);
                    AddContentToTextBlock(mentionLink);
                    break;
                }
            }
        }

        private void UpdateAvatar(string avatarUrl)
        {
            StatusAvatarImage.ImageSource = new BitmapImage(new Uri(avatarUrl));
        }

        private void UpdateNameTextBlocks(Account account)
        {
            DisplayNameTextBlock.Text = account.DisplayName;
            AccountNameTextBlock.Text = account.AccountName;
        }

        private void AddMediaToStatus(List<Attachment> mediaAttachments)
        {
            bool shouldNewParagraphBeCreated = false;
            for (int i = 0; i < mediaAttachments.Count; i++)
            {
                if (i == 0)
                {
                    shouldNewParagraphBeCreated = true;
                }

                InlineUIContainer mediaContainer = new InlineUIContainer();
                switch (mediaAttachments[i].Type)
                {
                    case MastoMediaConstants.VideoType:
                        MediaPlayerElement videoPlayer = new MediaPlayerElement
                        {
                            PosterSource = new BitmapImage(new Uri(mediaAttachments[i].PreviewUrl)),
                            Source = MediaSource.CreateFromUri(new Uri(mediaAttachments[i].Url)),
                            AreTransportControlsEnabled = true
                        };
                        videoPlayer.TransportControls.IsCompact = true;
                        mediaContainer.Child = videoPlayer;
                        break;

                    case MastoMediaConstants.GIFType:
                        MediaPlayerElement gifPlayer = new MediaPlayerElement
                        {
                            Source = MediaSource.CreateFromUri(new Uri(mediaAttachments[i].Url)),
                            AutoPlay = true,
                            AreTransportControlsEnabled = false,
                        };
                        gifPlayer.MediaPlayer.IsLoopingEnabled = true;
                        mediaContainer.Child = gifPlayer;
                        break;

                    default:
                        BitmapImage img = new BitmapImage(new Uri(mediaAttachments[i].PreviewUrl));
                        mediaContainer.Child = new Image { Source = img };
                        break;
                }

                AddContentToTextBlock(mediaContainer, shouldNewParagraphBeCreated);
                shouldNewParagraphBeCreated = false;
            }
        }

        private void AddContentToTextBlock(Inline content, bool doesANewParagraphNeedToBeCreated = false)
        {
            if (doesANewParagraphNeedToBeCreated)
            {
                var freshParagraph = new Paragraph();
                freshParagraph.Inlines.Add(content);
                StatusContent.Blocks.Add(freshParagraph);
            }
            else
            {

                // Only Block in UWP is a Paragraph
                Paragraph lastParagraph = (Paragraph)StatusContent.Blocks.Last();
                lastParagraph.Inlines.Add(content);
            }
        }

      

        private Status PickStatusFromReblogContext()
        {
            return CurrentStatus.Reblog == null ? CurrentStatus : CurrentStatus.Reblog; ;
        }

        private async void ReblogButton_Click(object sender, RoutedEventArgs e)
        {
            Status statusToUse = PickStatusFromReblogContext();

            try
            {

                Status reblogResult = null;

                if (statusToUse.Reblogged == true)
                {
                    reblogResult = await ClientHelper.Client.Unreblog(statusToUse.Id);
                }
                else if (statusToUse.Reblogged == false)
                {
                    reblogResult = await ClientHelper.Client.Reblog(statusToUse.Id);
                }
                statusToUse.Reblogged = reblogResult.Reblogged;
            }
            catch (Exception ex)
            {
                if (ex is Mastonet.ServerErrorException serverException)
                {
                    Debug.WriteLine(serverException);
                    statusToUse.Reblogged = !statusToUse.Reblogged;
                }
                else
                {
                    Debug.WriteLine("Reblog Failed, check internet connection!");
                    await ErrorService.ShowConnectionError();
                }

                ReblogButton.IsChecked = statusToUse.Reblogged;
            }
            UpdateTootRefVariableProperties();
        }

        private async void FavouriteButton_Click(object sender, RoutedEventArgs e)
        {
            Status statusToUse = PickStatusFromReblogContext();
            try
            {
                Status favResult = null;
                if (statusToUse.Favourited == true)
                {
                    favResult = await ClientHelper.Client.Unfavourite(statusToUse.Id);
                }
                else if (statusToUse.Favourited == false)
                {
                    favResult = await ClientHelper.Client.Favourite(statusToUse.Id);
                }
                statusToUse.Favourited = favResult.Favourited;

            }
            catch (Exception ex)
            {
                if (ex is Mastonet.ServerErrorException serverException)
                {
                    Debug.WriteLine(serverException);
                    statusToUse.Favourited = !statusToUse.Favourited;
                }
                else
                {
                    Debug.WriteLine("Favourite Failed, check internet connection!");
                    await ErrorService.ShowConnectionError();
                }
                FavouriteButton.IsChecked = statusToUse.Favourited;
                
            }
            UpdateTootRefVariableProperties();
        }

        private void UpdateTootRefVariableProperties()
        {
            CurrentStatus.TootRef.Reblogged = CurrentStatus.Reblogged;
            CurrentStatus.TootRef.Favourited = CurrentStatus.Favourited;
        }

        private async void ReplyButton_Click(object sender, RoutedEventArgs e)
        {
            Status statusToReturn = CurrentStatus.Reblog == null ? CurrentStatus : CurrentStatus.Reblog;
            NewReplyTootDialog dialog = new NewReplyTootDialog(statusToReturn);
            try
            {
                await dialog.ShowAsync();
            }
            catch
            {

            }
        }

    }
}

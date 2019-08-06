using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Tooter.Core;
using Tooter.Model;
using Tooter.Parsers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Tooter.LocalControls
{
    public sealed partial class TootTemplate : UserControl
    {
        public Status CurrentStatus { get { return this.DataContext as Status; } }
        public TootTemplate()
        {
            this.InitializeComponent();
            //this.DataContextChanged += (s, e) => this.Bindings.Update();
            this.DataContextChanged += UpdateData;


        }

        private void UpdateData(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            Status updatedStatus = (Status)args.NewValue;

            if (updatedStatus != null)
            {

                if (updatedStatus.Reblog != null)
                {
                    var reblogStatus = updatedStatus.Reblog;
                    if (reblogStatus.Content != null)
                    {
                        // On top of content, show that it's a reblog with indicators and stuff e.g (who reblogged
                        // on top of original status
                        UpdateNameTextBlocks(reblogStatus.Account);
                        UpdateAvatar(reblogStatus.Account.AvatarUrl);
                        RebloggedByButton.Visibility = Visibility.Visible;
                        RebloggedByButton.Content = $"Reblogged by: {updatedStatus.Account.DisplayName}";
                        StatusContent.Blocks.Clear();

                        StatusContent.Blocks.Clear();
                        MParser parser = new MParser();

                        Paragraph rootParagraph = new Paragraph();
                        StatusContent.Blocks.Add(rootParagraph);

                        List<MastoContent> parsedContent = null;
                        try
                        {
                            parsedContent = parser.ParseContent(reblogStatus.Content);
                            bool doesANewParagraphNeedToBeCreated = false;

                            for (int i = 0; i < parsedContent.Count; i++)
                            {
                                var item = parsedContent[i];


                                switch (item.ContentType)
                                {
                                    case MastoContentType.Mention:
                                        List<Mention> mentions = (List<Mention>)reblogStatus.Mentions;
                                        for (int mentionIndex = 0; mentionIndex < mentions.Count; mentionIndex++)
                                        {
                                            if (mentions[mentionIndex].AccountName == item.Content)
                                            {
                                                Run tagRun = new Run { Text = $"@{item.Content}" };
                                                Hyperlink mentionLink = new Hyperlink();
                                                mentionLink.Inlines.Add(tagRun);
                                                AddContentToTextBlock(mentionLink);
                                                break;
                                            }
                                        }
                                        break;
                                    case MastoContentType.Link:
                                        Run linkRun = new Run { Text = item.Content };
                                        Hyperlink link = new Hyperlink();
                                        link.NavigateUri = new Uri(item.Content);
                                        link.Inlines.Add(linkRun);
                                        AddContentToTextBlock(link);
                                        break;
                                    case MastoContentType.Text:
                                        var textItem = (MastoText)item;
                                        if (i == 0)
                                        {
                                            item.Content = item.Content.TrimStart();
                                        }

                                        if (textItem.IsParagraph)
                                        {
                                            doesANewParagraphNeedToBeCreated = true;
                                            Run run = new Run { Text = $"{textItem.Content}" };
                                            AddContentToTextBlock(run);
                                            doesANewParagraphNeedToBeCreated = true;
                                        }
                                        else
                                        {
                                            Run run = new Run { Text = textItem.Content };
                                            AddContentToTextBlock(run, doesANewParagraphNeedToBeCreated);
                                            doesANewParagraphNeedToBeCreated = false;
                                        }
                                        break;

                                    case MastoContentType.Hashtag:
                                        List<Tag> tags = (List<Tag>)reblogStatus.Tags;
                                        for (int tagIndex = 0; tagIndex < tags.Count; tagIndex++)
                                        {
                                            if (tags[tagIndex].Name == item.Content)
                                            {
                                                Run tagRun = new Run { Text = $"#{item.Content}" };
                                                Hyperlink hashtagLink = new Hyperlink();
                                                hashtagLink.Inlines.Add(tagRun);
                                                AddContentToTextBlock(hashtagLink);
                                                break;
                                            }
                                        }

                                        break;

                                    default:
                                        break;
                                }

                            }



                        }
                        catch
                        {
                            Run run = new Run { Text = $"ERROR!: {reblogStatus.Content}" };
                            run.Foreground = new SolidColorBrush(Colors.Red);
                            rootParagraph.Inlines.Add(run);
                        }




                        AddMediaToStatus((List<Attachment>)reblogStatus.MediaAttachments);
                    }

                }


                // Display regular status
                else
                {
                    UpdateNameTextBlocks(updatedStatus.Account);
                    UpdateAvatar(updatedStatus.Account.AvatarUrl);

                    RebloggedByButton.Visibility = Visibility.Collapsed;
                    if (updatedStatus.Content != null)
                    {
                        StatusContent.Blocks.Clear();
                        MParser parser = new MParser();

                        Paragraph rootParagraph = new Paragraph();
                        StatusContent.Blocks.Add(rootParagraph);

                        List<MastoContent> parsedContent = null;
                        try
                        {
                            parsedContent = parser.ParseContent(updatedStatus.Content);
                            bool doesANewParagraphNeedToBeCreated = false;

                            for (int i = 0; i < parsedContent.Count; i++)
                            {
                                var item = parsedContent[i];


                                switch (item.ContentType)
                                {
                                    case MastoContentType.Mention:
                                        List<Mention> mentions = (List<Mention>)updatedStatus.Mentions;
                                        for (int mentionIndex = 0; mentionIndex < mentions.Count; mentionIndex++)
                                        {
                                            if (mentions[mentionIndex].AccountName == item.Content)
                                            {
                                                Run tagRun = new Run { Text = $"@{item.Content}" };
                                                Hyperlink mentionLink = new Hyperlink();
                                                mentionLink.Inlines.Add(tagRun);
                                                AddContentToTextBlock(mentionLink);
                                                break;
                                            }
                                        }
                                        break;
                                    case MastoContentType.Link:
                                        Run linkRun = new Run { Text = item.Content };
                                        Hyperlink link = new Hyperlink();
                                        link.NavigateUri = new Uri(item.Content);
                                        link.Inlines.Add(linkRun);
                                        AddContentToTextBlock(link);
                                        break;
                                    case MastoContentType.Text:
                                        var textItem = (MastoText)item;
                                        if (i == 0)
                                        {
                                            item.Content = item.Content.TrimStart();
                                        }

                                        if (textItem.IsParagraph)
                                        {
                                            doesANewParagraphNeedToBeCreated = true;
                                            Run run = new Run { Text = $"{textItem.Content}" };
                                            AddContentToTextBlock(run);
                                            doesANewParagraphNeedToBeCreated = true;
                                        }
                                        else
                                        {
                                            Run run = new Run { Text = textItem.Content };
                                            AddContentToTextBlock(run, doesANewParagraphNeedToBeCreated);
                                            doesANewParagraphNeedToBeCreated = false;
                                        }
                                        break;

                                    case MastoContentType.Hashtag:
                                        List<Tag> tags = (List<Tag>)updatedStatus.Tags;
                                        for (int tagIndex = 0; tagIndex < tags.Count; tagIndex++)
                                        {
                                            if (tags[tagIndex].Name == item.Content)
                                            {
                                                Run tagRun = new Run { Text = $"#{item.Content}" };
                                                Hyperlink hashtagLink = new Hyperlink();
                                                hashtagLink.Inlines.Add(tagRun);
                                                AddContentToTextBlock(hashtagLink);
                                                break;
                                            }
                                        }

                                        break;

                                    default:
                                        break;
                                }

                            }



                        }
                        catch
                        {
                            Run run = new Run { Text = $"ERROR!: {updatedStatus.Content}" };
                            run.Foreground = new SolidColorBrush(Colors.Red);
                            rootParagraph.Inlines.Add(run);
                        }




                        AddMediaToStatus((List<Attachment>)updatedStatus.MediaAttachments);
                    }
                }
            }

            args.Handled = true;
        }

        private void UpdateAvatar(string avatarUrl)
        {
            StatusAvatar.ProfilePicture = new BitmapImage(new Uri(avatarUrl));
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
                        MediaPlayerElement videoPlayer = new MediaPlayerElement();
                        videoPlayer.PosterSource = new BitmapImage(new Uri(mediaAttachments[i].PreviewUrl));
                        videoPlayer.Source = MediaSource.CreateFromUri(new Uri(mediaAttachments[i].Url));
                        videoPlayer.AreTransportControlsEnabled = true;
                        videoPlayer.TransportControls.IsCompact = true;
                        mediaContainer.Child = videoPlayer;
                        break;
                    case MastoMediaConstants.GIFType:
                        MediaPlayerElement gifPlayer = new MediaPlayerElement();
                        gifPlayer.Source = MediaSource.CreateFromUri(new Uri(mediaAttachments[i].Url));
                        gifPlayer.AutoPlay = true;
                        gifPlayer.AreTransportControlsEnabled = false;
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
    }
}

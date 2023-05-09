using Mastonet.Entities;
using Bluechirp.Parser;
using Bluechirp.Parser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Bluechirp.Library.Helpers;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
using Bluechirp.Parser.Interfaces;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Bluechirp.LocalControls
{
    public sealed partial class QuotedTootTemplate : UserControl
    {
        public Status CurrentStatus { get { return this.DataContext as Status; } }
        public QuotedTootTemplate()
        {
            this.InitializeComponent();
            this.DataContextChanged += UpdateData;
        }

       

        private void UpdateData(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            Status updatedStatus = (Status)args.NewValue;

            if (updatedStatus != null)
            {

                StatusContent.Blocks.Clear();

                Paragraph rootParagraph = new Paragraph();
                StatusContent.Blocks.Add(rootParagraph);

                TootParser parser = new TootParser();

                // Display regular status

                if (updatedStatus.Content != null)
                {
                    UpdateTimestamp(updatedStatus.CreatedAt);
                    try
                    {
                        List<IMastodonContent> parsedContent = AsyncHelper.RunSync(() => parser.ParseContentAsync(updatedStatus.Content));
                        TryDisplayParsedContent(parsedContent, updatedStatus);
                    }
                    catch
                    {
                        Run run = new Run { Text = $"ERROR!: {updatedStatus.Content}" };
                        run.Foreground = new SolidColorBrush(Colors.Red);
                        rootParagraph.Inlines.Add(run);
                    }
                }

            }
            args.Handled = true;

        }


        private void UpdateTimestamp(DateTime createdAt)
        {
            StatusTimestamp.Text = TimestampHelper.FormatTimestamp(createdAt);
        }


        private void TryDisplayParsedContent(List<IMastodonContent> parsedContent, Status status)
        {
            bool doesANewParagraphNeedToBeCreated = false;
            for (int i = 0; i < parsedContent.Count; i++)
            {
                var item = parsedContent[i];
                switch (item.ContentType)
                {
                    case MastodonContentType.Mention:
                        List<Mention> mentions = (List<Mention>)status.Mentions;
                        TryAddMentions(mentions, item.Content);
                        break;
                    case MastodonContentType.Link:
                        TryAddLinks(item.Content);
                        break;
                    case MastodonContentType.Text:
                        var textItem = (MastodonText)item;
                        TryAddText(textItem, i, ref doesANewParagraphNeedToBeCreated);
                        break;
                    case MastodonContentType.Hashtag:
                        List<Tag> tags = (List<Tag>)status.Tags;
                        TryAddHashtags(tags, item.Content);
                        break;
                    default:
                        break;
                }

            }
        }

        private void TryAddHashtags(List<Tag> tags, string contentValue)
        {
            for (int tagIndex = 0; tagIndex < tags.Count; tagIndex++)
            {
                if (tags[tagIndex].Name == contentValue)
                {
                    Run tagRun = new Run { Text = $"#{contentValue}" };
                    AddContentToTextBlock(tagRun);
                    break;
                }
            }
        }

        private void TryAddText(MastodonText textItem, int loopsCompleted, ref bool doesANewParagraphNeedToBeCreated)
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
            AddContentToTextBlock(linkRun);
        }

        private void TryAddMentions(List<Mention> mentions, string contentValue)
        {

            Run tagRun = new Run { Text = $"@{contentValue}" };
            AddContentToTextBlock(tagRun);

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

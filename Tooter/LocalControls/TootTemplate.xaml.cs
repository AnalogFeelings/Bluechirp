using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Tooter.Model;
using Tooter.Parsers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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

                            if (item.ContentType == Model.MastoContentType.Text)
                            {
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
                            }
                        }



                    }
                    catch
                    {
                        Run run = new Run { Text = $"ERROR!: {updatedStatus.Content}" };
                        run.Foreground = new SolidColorBrush(Colors.Red);
                        rootParagraph.Inlines.Add(run);
                    }




                }
            }
            Bindings.Update();
            args.Handled = true;
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

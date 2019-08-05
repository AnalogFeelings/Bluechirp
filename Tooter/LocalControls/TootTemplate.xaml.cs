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

                    List<MastoContent> parsedContent = null;
                    try
                    {
                        parsedContent = parser.ParseContent(updatedStatus.Content);

                        foreach (var item in parsedContent)
                        {
                            if (item.ContentType == Model.MastoContentType.Text)
                            {
                                var textItem = (MastoText)item;
                                if (textItem.IsParagraph)
                                {
                                    Run run = new Run { Text = $"{textItem.Content}\n\n" };
                                    rootParagraph.Inlines.Add(run);
                                }
                                else
                                {
                                    Run run = new Run { Text = textItem.Content };
                                    rootParagraph.Inlines.Add(run);
                                }
                            }


                        }
                    }
                    catch
                    {
                        Run run = new Run { Text = $"ERROR!: {updatedStatus.Content}"};
                        run.Foreground = new SolidColorBrush(Colors.Red);
                        rootParagraph.Inlines.Add(run);
                    }



                    StatusContent.Blocks.Add(rootParagraph);
                }
            }
            Bindings.Update();
            args.Handled = true;
        }
    }
}

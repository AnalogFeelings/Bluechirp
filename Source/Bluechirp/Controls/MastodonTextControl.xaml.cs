using Bluechirp.Library.Enums;
using Bluechirp.Library.Helpers;
using Bluechirp.Library.Models;
using Bluechirp.Library.Services.Environment;
using Bluechirp.Library.Services.Mastodon;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bluechirp.Controls;

/// <summary>
/// Custom control to display Mastodon HTML content inside
/// a rich text control.
/// </summary>
public sealed partial class MastodonTextControl : UserControl
{
    public static readonly DependencyProperty HtmlProperty = DependencyProperty.Register(nameof(Html),
        typeof(string), typeof(MastodonTextControl), new PropertyMetadata(null, OnHtmlChanged));

    public string Html
    {
        get => (string)GetValue(HtmlProperty);
        set => SetValue(HtmlProperty, value);
    }

    private IMastodonTextParserService _parserService;
    private IDispatcherService _dispatcherService;

    public MastodonTextControl()
    {
        this.InitializeComponent();
        _parserService = App.ServiceProvider.GetRequiredService<IMastodonTextParserService>();
        _dispatcherService = App.ServiceProvider.GetRequiredService<IDispatcherService>();
    }

    /// <summary>
    /// Updates the content inside <see cref="ContentParagraph"/>.
    /// </summary>
    private async Task UpdateContent()
    {
        ContentParagraph.Inlines.Clear();

        List<MastodonContent> parsedContent = await _dispatcherService.EnqueueAsync(() => _parserService.ParseHtmlAsync(Html));

        foreach (MastodonContent content in parsedContent)
        {
            switch(content.ContentType)
            {
                case MastodonContentType.Mention:
                    Hyperlink mentionLink = new Hyperlink();
                    Run mentionRun = new Run()
                    {
                        Text = "@" + content.Content
                    };

                    mentionLink.Inlines.Add(mentionRun);

                    ContentParagraph.Inlines.Add(mentionLink);
                    break;
                case MastodonContentType.Link:
                    Hyperlink linkHyperlink = new Hyperlink()
                    {
                        NavigateUri = new Uri(content.Content)
                    };
                    Run linkRun = new Run()
                    {
                        Text = content.Content
                    };

                    linkHyperlink.Inlines.Add(linkRun);

                    ContentParagraph.Inlines.Add(linkHyperlink);
                    break;
                case MastodonContentType.Text:
                    Run textRun = new Run()
                    {
                        Text = content.Content
                    };
                    ContentParagraph.Inlines.Add(textRun);
                    break;
                case MastodonContentType.Hashtag:
                    Hyperlink hashtagLink = new Hyperlink();
                    Run hashtagRun = new Run()
                    {
                        Text = "#" + content.Content
                    };

                    hashtagLink.Inlines.Add(hashtagRun);

                    ContentParagraph.Inlines.Add(hashtagLink);
                    break;
            }
        }
    }

    private static async void OnHtmlChanged(DependencyObject @object,  DependencyPropertyChangedEventArgs e)
    {
        MastodonTextControl textControl = (@object as MastodonTextControl)!;

        await textControl.UpdateContent();
    }
}

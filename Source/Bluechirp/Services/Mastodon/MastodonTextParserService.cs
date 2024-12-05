#region License Information (GPLv3)
// Bluechirp - A modern, native client for the Mastodon social media.
// Copyright (C) 2023 Analog Feelings and contributors.
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
#endregion

using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Bluechirp.Library.Enums;
using Bluechirp.Library.Models;
using Bluechirp.Library.Services.Mastodon;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bluechirp.Services.Mastodon;

/// <summary>
/// Implements an HTML Mastodon content parser service.
/// </summary>
internal class MastodonTextParserService : IMastodonTextParserService
{
    /// <inheritdoc/>
    /// <exception cref="InvalidDataException">Thrown if a top level element is not a paragraph tag.</exception>
    public async Task<List<MastodonContent>> ParseHtmlAsync(string htmlContent)
    {
        IBrowsingContext browsingContext = new BrowsingContext();
        IDocument parsedContent = await browsingContext.OpenAsync(x => x.Content(htmlContent));
        List<MastodonContent> contentList = new List<MastodonContent>();

        if (parsedContent.Body == null || parsedContent.Body.ChildElementCount == 0)
        {
            // For some reason, sometimes no HTML is returned, only plain text. 
            MastodonContent plainText = new MastodonContent()
            {
                Content = htmlContent,
                ContentType = MastodonContentType.Text
            };

            contentList.Add(plainText);
        }
        else
        {
            IEnumerable<IElement> topLevelElements = parsedContent.Body.Children.Where(x => x.Parent == parsedContent.Body);

            foreach (IElement topLevelElement in topLevelElements)
            {
                switch(topLevelElement.NodeName.ToLower())
                {
                    case "p":
                        HandleParagraphTag(topLevelElement as IHtmlParagraphElement, ref contentList);

                        if (!topLevelElement.IsLastChild())
                        {
                            MastodonContent newLineText = new MastodonContent()
                            {
                                Content = "\n\n",
                                ContentType = MastodonContentType.Text
                            };

                            contentList.Add(newLineText);
                        }
                        break;
                    case "#text":
                        HandleRawText(topLevelElement as IText, ref contentList); 
                        break;
                    case "a":
                        HandleAnchorTag(topLevelElement as IHtmlAnchorElement, ref contentList);
                        break;
                    case "br":
                        HandleLineBreak(ref contentList);
                        break;
                    case "span":
                        HandleSpanTag(topLevelElement as IHtmlSpanElement, ref contentList);
                        break;
                }
            }
        }

        return contentList;
    }

    /// <summary>
    /// Parses a paragraph <see cref="IElement"/>.
    /// </summary>
    /// <param name="paragraph">The paragraph element.</param>
    /// <param name="outputList">A reference to the output content list.</param>
    /// <exception cref="InvalidDataException">Thrown if the mention element lacks the necessary children.</exception>
    private void HandleParagraphTag(IHtmlParagraphElement paragraph, ref List<MastodonContent> outputList)
    {
        if (!paragraph.HasChildNodes)
            throw new InvalidDataException("Toot paragraph was somehow empty, Mastodon bug?");

        foreach(INode element in paragraph.ChildNodes)
        {
            switch(element.NodeName.ToLower())
            {
                case "#text":
                    HandleRawText(element as IText, ref outputList); 
                    break;
                case "a":
                    HandleAnchorTag(element as IHtmlAnchorElement, ref outputList);
                    break;
                case "br":
                    HandleLineBreak(ref outputList);
                    break;
                case "span":
                    HandleSpanTag(element as IHtmlSpanElement, ref outputList);
                    break;
            }
        }
    }

    /// <summary>
    /// Parses a mention <see cref="IElement"/>.
    /// </summary>
    /// <param name="mention">The mention element.</param>
    /// <param name="outputList">A reference to the output content list.</param>
    /// <exception cref="InvalidDataException">Thrown if the mention element lacks the necessary children.</exception>
    private void HandleSpanTag(IHtmlSpanElement mention, ref List<MastodonContent> outputList)
    {
        // Bingo.
        if (mention.ClassList.Contains("h-card"))
        {
            IHtmlAnchorElement mentionElement = mention.FindChild<IHtmlAnchorElement>();

            if (mentionElement == null)
                throw new InvalidDataException("Mention did not contain a child anchor node.");

            if (mentionElement.ClassList.Contains("mention"))
            {
                // Why is it this way.
                IHtmlSpanElement mentionSpan = mentionElement.FindChild<IHtmlSpanElement>();

                if (mentionSpan == null)
                    throw new InvalidDataException("Mention did not contain text node.");

                MastodonContent mentionText = new MastodonContent()
                {
                    Content = mentionSpan.Text(),
                    ContentType = MastodonContentType.Mention
                };

                outputList.Add(mentionText);
            }
        }
    }

    /// <summary>
    /// Parses a line break <see cref="IElement"/>.
    /// </summary>
    /// <param name="outputList">A reference to the output content list.</param>
    private void HandleLineBreak(ref List<MastodonContent> outputList)
    {
        MastodonContent newLineText = new MastodonContent()
        {
            Content = "\n",
            ContentType = MastodonContentType.Text
        };

        outputList.Add(newLineText);
    }

    /// <summary>
    /// Parses a text <see cref="IElement"/>.
    /// </summary>
    /// <param name="text">The text element.</param>
    /// <param name="outputList">A reference to the output content list.</param>
    private void HandleRawText(IText text, ref List<MastodonContent> outputList)
    {
        MastodonContent textContent = new MastodonContent()
        {
            Content = text.TextContent,
            ContentType = MastodonContentType.Text
        };

        outputList.Add(textContent);
    }

    /// <summary>
    /// Parses a hashtag or link element <see cref="INode"/>.
    /// </summary>
    /// <param name="anchor">The anchor element.</param>
    /// <param name="outputList">A reference to the output content list.</param>
    /// <exception cref="InvalidDataException">Thrown if the mention element lacks the necessary children.</exception>
    private void HandleAnchorTag(IHtmlAnchorElement anchor, ref List<MastodonContent> outputList)
    {
        // This is a hashtag.
        if (anchor.ClassList.Contains("mention") && anchor.ClassList.Contains("hashtag"))
        {
            IHtmlSpanElement childSpan = anchor.FindChild<IHtmlSpanElement>();

            if (childSpan == null)
                throw new InvalidDataException("Hashtag did not contain a child span node.");

            MastodonContent hashtagLink = new MastodonContent()
            {
                Content = childSpan.Text(),
                ContentType = MastodonContentType.Hashtag
            };

            outputList.Add(hashtagLink);
        }
        else // This is a plain link.
        {
            MastodonContent plainLink = new MastodonContent()
            {
                Content = anchor.Href,
                ContentType = MastodonContentType.Link
            };

            outputList.Add(plainLink);
        }
    }
}

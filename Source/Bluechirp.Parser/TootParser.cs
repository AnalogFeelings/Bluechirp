using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Bluechirp.Parser.Interfaces;
using Bluechirp.Parser.Model;

namespace Bluechirp.Parser
{
    public class TootParser
    {
        public async Task<List<IMastodonContent>> ParseContentAsync(string HtmlContent)
        {
            IBrowsingContext browsingContext = new BrowsingContext();
            IDocument parsedContent = await browsingContext.OpenAsync(x => x.Content(HtmlContent));
            List<IMastodonContent> contentList = new List<IMastodonContent>();

            if (parsedContent.Body == null || parsedContent.Body.ChildElementCount == 0)
            {
                // For some reason, sometimes no HTML is returned, only plain text. 
                MastodonText plainText = new MastodonText(HtmlContent);

                contentList.Add(plainText);

                return contentList;
            }

            IEnumerable<IElement> topLevelElements = parsedContent.Body.Children.Where(x => x.Parent == parsedContent.Body);

            // Iterate over all top-level elements.
            foreach (IElement element in topLevelElements)
                switch (element.NodeName.ToLower())
                {
                    case ParserConstants.PARAGRAPH_TAG: // This is a paragraph.
                        HandleParagraphTag(element, ref contentList);

                        // HACK: Maybe this is ugly?
                        if (!element.IsLastChild())
                        {
                            MastodonText newLineText = new MastodonText("\n\n");

                            contentList.Add(newLineText);
                        }

                        break;
                }

            return contentList;
        }

        private void HandleParagraphTag(IElement Paragraph, ref List<IMastodonContent> OutputList)
        {
            // Sanity check.
            if (!Paragraph.HasChildNodes)
                throw new InvalidDataException("Toot paragraph was somehow empty, could be a bug in Mastodon's server.");

            foreach (INode childNode in Paragraph.ChildNodes)
                switch (childNode.NodeName.ToLower())
                {
                    case ParserConstants.RAW_TEXT_TAG: // This is plain text.
                        HandleRawText(childNode, ref OutputList);
                        break;
                    case ParserConstants.LINK_TAG: // This has to be a hashtag or plain link.
                        HandleAnchorTag(childNode, ref OutputList);
                        break;
                    case ParserConstants.BREAK_TAG: // This is just a plain line break.
                        HandleLineBreak(childNode, ref OutputList);
                        break;
                    case ParserConstants.SPAN_TAG: // This is most likely a mention.
                        HandleMention(childNode, ref OutputList);
                        break;
                }
        }

        private void HandleMention(INode Mention, ref List<IMastodonContent> OutputList)
        {
            IHtmlSpanElement spanElement = Mention as IHtmlSpanElement;

            // Bingo.
            if (spanElement.ClassList.Contains(ParserConstants.MENTION_SPAN_CLASS))
            {
                IHtmlAnchorElement mentionElement = spanElement.FindChild<IHtmlAnchorElement>();

                if (mentionElement == null)
                    throw new InvalidDataException("Mention did not contain a child anchor node.");

                if (mentionElement.ClassList.Contains(ParserConstants.MENTION_CLASS) &&
                    !mentionElement.ClassList.Contains(ParserConstants.HASHTAG_CLASS))
                {
                    // Why is it this way.
                    IHtmlSpanElement mentionSpan = mentionElement.FindChild<IHtmlSpanElement>();

                    if (mentionSpan == null)
                        throw new InvalidDataException("Mention did not contain text node.");

                    MastodonMention mentionText = new MastodonMention(mentionSpan.Text());

                    OutputList.Add(mentionText);
                }
            }
        }

        private void HandleLineBreak(INode Break, ref List<IMastodonContent> OutputList)
        {
            MastodonText breakText = new MastodonText("\n");

            OutputList.Add(breakText);
        }

        private void HandleRawText(INode Paragraph, ref List<IMastodonContent> OutputList)
        {
            // TODO: Parse emojis, for some reason they're not returned as HTML nodes but as raw text.

            MastodonText rawText = new MastodonText(Paragraph.TextContent);

            OutputList.Add(rawText);
        }

        private void HandleAnchorTag(INode Anchor, ref List<IMastodonContent> OutputList)
        {
            IHtmlAnchorElement anchorElement = Anchor as IHtmlAnchorElement;

            if (anchorElement.ClassList.Contains(ParserConstants.MENTION_CLASS))
            {
                // This is a hashtag.
                if (anchorElement.ClassList.Contains(ParserConstants.HASHTAG_CLASS))
                {
                    IHtmlSpanElement childSpan = anchorElement.FindChild<IHtmlSpanElement>();

                    if (childSpan == null)
                        throw new InvalidDataException("Hashtag did not contain a child span node.");

                    MastodonHashtag hashtagLink = new MastodonHashtag(childSpan.Text());

                    OutputList.Add(hashtagLink);
                }
            }
            else // This is a plain link.
            {
                string targetUrl = anchorElement.Href;
                MastodonLink plainLink = new MastodonLink(targetUrl);

                OutputList.Add(plainLink);
            }
        }
    }
}
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
        //private const char _SPACE_CHAR = (char) 32;

        //private Queue<char> _charQueue;
        //private readonly StringBuilder _parseBuffer = new StringBuilder();

        /* <p>gonna send this toot to test bluechirp&#39;s new parser :ablobcatbongo:</p>
           <p>
            <a href="https://tech.lgbt/tags/hashtags" class="mention hashtag" rel="tag">#<span>hashtags</span></a>
            <span class="h-card">
                <a href="https://hachyderm.io/@witchdagger" class="u-url mention">@<span>witchdagger</span></a>
            </span>
           </p>
        */
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
            {
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
            }

            return contentList;
        }

        private void HandleParagraphTag(IElement Paragraph, ref List<IMastodonContent> OutputList)
        {
            // Sanity check.
            if (!Paragraph.HasChildNodes)
                throw new InvalidDataException("Toot paragraph was somehow empty, could be a bug in Mastodon's server.");

            foreach (INode childNode in Paragraph.ChildNodes)
            {
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

        //private List<IMastodonContent> ParseLoop(string Tag)
        //{
        //    string parsedTag = string.Empty;

        //    List<IMastodonContent> parsedContent = new List<IMastodonContent>();

        //    while (LoopConditionIsTrue(Tag, parsedTag))
        //    {
        //        bool inBreakTag = CheckIfTag(parsedTag, ParserConstants.BREAK_TAG);
        //        char character = _charQueue.Dequeue();

        //        if (inBreakTag)
        //        {
        //            if (character == ParserConstants.TAG_END_CHARACTER)
        //            {
        //                TryAddTextToParsedContent(parsedContent, "\n");

        //                ClearTag(ref parsedTag);
        //            }
        //        }
        //        else if (parsedTag != string.Empty)
        //        {
        //            if (CheckIfTag(parsedTag, $"{ParserConstants.LINK_TAG}"))
        //            {
        //                parsedContent.Add(HandleLinkTag());
        //            }
        //            else
        //            {
        //                if (CheckIfTag(parsedTag, $"{ParserConstants.PARAGRAPH_TAG}")) TryAddTextToParsedContent(parsedContent, "\n\n");

        //                // Parse through inner content of parsed tag.
        //                List<IMastodonContent> recursiveContent = ParseLoop(parsedTag);
        //                parsedContent.AddRange(recursiveContent);
        //            }

        //            ClearTag(ref parsedTag);
        //        }
        //        else
        //        {
        //            // Parse through inner content between open and close tags
        //            (bool hasTextToParse, bool hasTagToParse, string text, string tag) textHandlingResult = HandleText(character);

        //            if (textHandlingResult.hasTextToParse) TryAddTextToParsedContent(parsedContent, textHandlingResult.text);

        //            if (textHandlingResult.hasTagToParse) parsedTag = textHandlingResult.tag;
        //        }
        //    }

        //    if (_parseBuffer.Length > 0) TryAddTextToParsedContent(parsedContent, _parseBuffer.ToString());

        //    return parsedContent;
        //}

        //private void ClearTag(ref string ParsedTag)
        //{
        //    ParsedTag = string.Empty;
        //}

        //private void TryAddTextToParsedContent(List<IMastodonContent> ParsedContent, string ContentToAdd, bool IsParagraph = true)
        //{
        //    ContentToAdd = ContentToAdd.Replace(">", "");

        //    if (ContentToAdd != string.Empty) ParsedContent.Add(new MastodonText(WebUtility.HtmlDecode(ContentToAdd)));
        //}

        //private bool CheckIfTag(string ParsedTag, string ExpectedTag)
        //{
        //    return ParsedTag == ExpectedTag;
        //}

        //private bool LoopConditionIsTrue(string Tag, string ParsedTag)
        //{
        //    bool willLoopContinue;
        //    if (Tag == string.Empty)
        //        willLoopContinue = _charQueue.Count > 0;
        //    else
        //        willLoopContinue = ParsedTag != $"/{Tag}";

        //    return willLoopContinue;
        //}

        //private (bool hasTextToParse, bool hasTagToParse, string text, string tag) HandleText(char Character)
        //{
        //    bool hasTextToParse = false;
        //    bool hasTagToParse = false;
        //    string text = null;
        //    string tag = null;

        //    if (Character == ParserConstants.TAG_START_CHARACTER)
        //    {
        //        if (_parseBuffer.Length > 0)
        //        {
        //            text = _parseBuffer.ToString();
        //            _parseBuffer.Clear();
        //            hasTextToParse = true;
        //        }

        //        hasTagToParse = true;
        //    }

        //    if (hasTagToParse)
        //        tag = ParseTag();
        //    else
        //        _parseBuffer.Append(Character);

        //    return (hasTextToParse, hasTagToParse, text, tag);
        //}

        //private string ParseTag()
        //{
        //    // '<' has been removed from queue already.
        //    // so you just need to add all characters before a
        //    // (space).

        //    StringBuilder parsedTagBuffer = new StringBuilder();
        //    while (_charQueue.Peek() != _SPACE_CHAR && _charQueue.Peek() != '>')
        //    {
        //        char thisChar = _charQueue.Dequeue();
        //        parsedTagBuffer.Append(thisChar);
        //    }

        //    // Code for skipping over span attributes
        //    if (parsedTagBuffer.ToString().Contains(ParserConstants.SPAN_TAG))
        //    {
        //        while (_charQueue.Peek() != '>')
        //            _charQueue.Dequeue();
        //    }

        //    return parsedTagBuffer.ToString();
        //}

        //private Dictionary<string, string> FindAttributes()
        //{
        //    Dictionary<string, string> tagAttributes = new Dictionary<string, string>();
        //    StringBuilder attributeBuffer = new StringBuilder();
        //    string currentAttribute = "";
        //    bool hasTagBeenClosed = false;
        //    bool isInAttributeValue = false;

        //    while (!hasTagBeenClosed)
        //    {
        //        char character = _charQueue.Dequeue();
        //        if (character == _SPACE_CHAR && isInAttributeValue == false)
        //        {
        //            if (currentAttribute != string.Empty)
        //            {
        //                tagAttributes[currentAttribute] = attributeBuffer.ToString();
        //                currentAttribute = string.Empty;
        //                attributeBuffer.Clear();
        //            }
        //        }
        //        else if (character == '>')
        //        {
        //            // First tag has been closed
        //            hasTagBeenClosed = true;

        //            if (currentAttribute != string.Empty)
        //            {
        //                tagAttributes[currentAttribute] = attributeBuffer.ToString();
        //                currentAttribute = string.Empty;
        //                attributeBuffer.Clear();
        //            }
        //        }
        //        else
        //        {
        //            // Fill in attribute name
        //            if (currentAttribute == string.Empty)
        //            {
        //                if (character == '=')
        //                {
        //                    currentAttribute = attributeBuffer.ToString();
        //                    attributeBuffer.Clear();
        //                }
        //                else
        //                {
        //                    attributeBuffer.Append(character);
        //                }
        //            }
        //            else // currentAttribute has been filled, handle attribute values now
        //            {
        //                if (character == '"')
        //                    isInAttributeValue = !isInAttributeValue;
        //                else
        //                    attributeBuffer.Append(character);
        //            }
        //        }
        //    }

        //    return tagAttributes;
        //}

        //public IMastodonContent HandleLinkTag()
        //{
        //    IMastodonContent contentToReturn = null;

        //    Dictionary<string, string> tagAttributes = FindAttributes();

        //    // Now try to take action depending on the result of trying to find the "class" attribute
        //    bool hasClassAttribute = tagAttributes.ContainsKey(ParserConstants.CLASS_ATTRIBUTE);
        //    bool isUniqueLink = false;

        //    string classAttributeValue = string.Empty;

        //    if (hasClassAttribute)
        //    {
        //        classAttributeValue = tagAttributes[ParserConstants.CLASS_ATTRIBUTE];
        //        if (classAttributeValue != string.Empty) isUniqueLink = true;
        //    }

        //    if (isUniqueLink)
        //    {
        //        // handle a mention/hashtag.
        //        switch (classAttributeValue)
        //        {
        //            case ParserConstants.HASHTAG_CLASS:
        //                contentToReturn = ParseUniqueLink('#');
        //                break;
        //            case ParserConstants.MENTION_CLASS:
        //                contentToReturn = ParseUniqueLink('@');
        //                break;
        //            case ParserConstants.PLAIN_HASHTAG_CLASS:
        //                contentToReturn = PlainHashtagParse();
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        // Do regular link stuff
        //        contentToReturn = new IMastodonContent(tagAttributes[ParserConstants.LINK_HREF], MastoContentType.Link);
        //        SkipToLinkTagEnd();
        //    }

        //    return contentToReturn;
        //}

        //private IMastodonContent PlainHashtagParse()
        //{
        //    StringBuilder plainHashtagBuffer = new StringBuilder();

        //    while (true)
        //    {
        //        char charFound = _charQueue.Dequeue();
        //        if (charFound == '<')
        //            break;
        //        if (charFound != '#') plainHashtagBuffer.Append(charFound);
        //    }

        //    IMastodonContent contentToReturn = new IMastodonContent(plainHashtagBuffer.ToString(), MastoContentType.Hashtag);

        //    return contentToReturn;
        //}

        //private IMastodonContent ParseUniqueLink(char UniqueChar)
        //{
        //    IMastodonContent contentToReturn = null;
        //    bool wasUniqueCharFound = false;
        //    bool linkTagEndReached = false;
        //    bool spanTagReached = false;
        //    bool isInSpanTagContent = false;

        //    StringBuilder uniqueLinkBuffer = new StringBuilder();

        //    while (!linkTagEndReached)
        //    {
        //        char charFound = _charQueue.Dequeue();
        //        if (wasUniqueCharFound == false)
        //        {
        //            if (charFound == UniqueChar) wasUniqueCharFound = true;
        //        }
        //        else if (!spanTagReached)
        //        {
        //            if (charFound == '<') spanTagReached = true;
        //        }
        //        else if (isInSpanTagContent)
        //        {
        //            if (charFound == '<')
        //            {
        //                // Save buffer content
        //                // clear buffer
        //                // dequeue to link tag end
        //                // set isLinkTagReached to true
        //                string uniqueLinkContent = WebUtility.HtmlDecode(uniqueLinkBuffer.ToString());
        //                MastoContentType contentType = DetermineContentTypeFromChar(UniqueChar);
        //                contentToReturn = new IMastodonContent(uniqueLinkContent, contentType);
        //                uniqueLinkBuffer.Clear();
        //                SkipToLinkTagEnd();
        //                linkTagEndReached = true;
        //            }
        //            else
        //            {
        //                uniqueLinkBuffer.Append(charFound);
        //            }
        //        }
        //        else if (spanTagReached)
        //        {
        //            if (charFound == '>') isInSpanTagContent = true;
        //        }
        //    }

        //    return contentToReturn;
        //}

        //private void SkipToLinkTagEnd()
        //{
        //    StringBuilder stringBuffer = new StringBuilder();

        //    while (!stringBuffer.ToString().Contains($"</{ParserConstants.LINK_TAG}>")) stringBuffer.Append(_charQueue.Dequeue());
        //}

        //private MastoContentType DetermineContentTypeFromChar(char UniqueChar)
        //{
        //    if (UniqueChar == '#')
        //        return MastoContentType.Hashtag;

        //    return MastoContentType.Mention;
        //}
    }
}
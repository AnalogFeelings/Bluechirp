using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using MastoParserLib.Model;

namespace MastoParserLib
{
    public class MParser
    {

        StringBuilder _parseBuffer = new StringBuilder();
        Queue<char> charQueue = new Queue<char>();
        const char SpaceChar = (char)32;


        private List<MastoContent> ParseLoop(string tag)
        {
            string parsedTag = String.Empty;
            bool inBreakTag;

            List<MastoContent> parsedContent = new List<MastoContent>();


            while (LoopConditionIsTrue(tag, parsedTag))
            {
                inBreakTag = CheckIfBreakTag(parsedTag);

                char character = charQueue.Dequeue();
                if (inBreakTag)
                {
                    if (character == ParserConstants.TagEndCharacter)
                    {
                        TryAddTextToParsedContent(parsedContent, "\n");
                        parsedTag = string.Empty;
                    }
                }

                else if (parsedTag != string.Empty)
                {
                    if (checkIfLinkTag(parsedTag))
                    {
                        parsedContent.Add(HandleLinkTag());
                    }
                    else
                    {
                        if (CheckIfParagraphTag(parsedTag))
                        {
                            TryAddTextToParsedContent(parsedContent, "\n\n");
                        }

                        // Parse through inner content of parsed tag.
                        var recursiveContent = ParseLoop(parsedTag);
                        parsedContent.AddRange(recursiveContent);
                    }
                    parsedTag = string.Empty;
                }
                else
                {
                    // Parse through inner content between open and close tags
                    var textHandlingResult = HandleText(character);

                    if (textHandlingResult.hasTextToParse)
                    {
                        TryAddTextToParsedContent(parsedContent, textHandlingResult.text);
                    }

                    if (textHandlingResult.hasTagToParse)
                    {
                        parsedTag = textHandlingResult.tag;
                    }
                }
            }

            if (_parseBuffer.Length > 0)
            {
                TryAddTextToParsedContent(parsedContent, _parseBuffer.ToString());

            }
            return parsedContent;
        }

        private bool CheckIfParagraphTag(string parsedTag)
        {
            return parsedTag == $"{ParserConstants.ParagraphTag}";
        }

        private void TryAddTextToParsedContent(List<MastoContent> parsedContent, string contentToAdd, bool isParagraph = true)
        {
            contentToAdd = contentToAdd.Replace(">", "");
            if (contentToAdd != string.Empty)
            {

                parsedContent.Add(new MastoText(WebUtility.HtmlDecode(contentToAdd)));
            }
        }

        private bool checkIfLinkTag(string parsedTag)
        {
            return parsedTag == $"{ParserConstants.LinkTag}";
        }

        private bool LoopConditionIsTrue(string tag, string parsedTag)
        {
            bool willLoopContinue;
            if (tag == string.Empty)
            {
                willLoopContinue = charQueue.Count > 0;

            }
            else
            {
                willLoopContinue = parsedTag != $"/{tag}";
            }

            return willLoopContinue;
        }

        private bool CheckIfBreakTag(string currentTag)
        {
            return currentTag.Equals(ParserConstants.BreakTag);
        }

        public List<MastoContent> ParseContent(string htmlContent)
        {
            charQueue = new Queue<char>(htmlContent);

            // At first, you'll start with no tag
            List<MastoContent> parsedContent = ParseLoop(string.Empty);

            return parsedContent;
        }

        private (bool hasTextToParse, bool hasTagToParse, string text, string tag) HandleText(char character)
        {
            bool hasTextToParse = false;
            bool hasTagToParse = false;
            string text = null;
            string tag = null;

            if (character == ParserConstants.TagStartCharacter)
            {
                if (_parseBuffer.Length > 0)
                {
                    text = _parseBuffer.ToString();
                    _parseBuffer.Clear();
                    hasTextToParse = true;
                }

                hasTagToParse = true;
            }

            if (hasTagToParse)
            {
                tag = ParseTag();
            }
            else
            {
                _parseBuffer.Append(character);
            }

            return (hasTextToParse, hasTagToParse, text, tag);
        }

        private string ParseTag()
        {
            // '<' has been removed from queue already.
            // so you just need to add all characters before a
            // (space).

            StringBuilder parsedTagBuffer = new StringBuilder();
            while (charQueue.Peek() != SpaceChar && charQueue.Peek() != '>')
            {
                char thisChar = charQueue.Dequeue();
                parsedTagBuffer.Append(thisChar);
            }

            // Code for skipping over span attributes
            if (parsedTagBuffer.ToString().Contains(ParserConstants.SpanTag))
            {
                while (charQueue.Peek() != '>')
                {
                    charQueue.Dequeue();
                }
            }
            return parsedTagBuffer.ToString();
        }

        private Dictionary<string, string> FindAttributes()
        {
            Dictionary<string, string> tagAttributes = new Dictionary<string, string>();
            StringBuilder attributeBuffer = new StringBuilder();
            string currentAttribute = "";
            bool hasTagBeenClosed = false;
            bool isInAttributeValue = false;


            while (!hasTagBeenClosed)
            {
                char character = charQueue.Dequeue();
                if (character == SpaceChar && isInAttributeValue == false)
                {
                    if (currentAttribute != string.Empty)
                    {
                        tagAttributes[currentAttribute] = attributeBuffer.ToString();
                        currentAttribute = string.Empty;
                        attributeBuffer.Clear();
                    }
                }

                else if (character == '>')
                {
                    // First tag has been closed
                    hasTagBeenClosed = true;
                    if (currentAttribute != string.Empty)
                    {
                        tagAttributes[currentAttribute] = attributeBuffer.ToString();
                        currentAttribute = string.Empty;
                        attributeBuffer.Clear();
                    }
                }

                else
                {
                    // Fill in attribute name
                    if (currentAttribute == string.Empty)
                    {

                        if (character == '=')
                        {
                            currentAttribute = attributeBuffer.ToString();
                            attributeBuffer.Clear();
                        }
                        else
                        {
                            attributeBuffer.Append(character);
                        }
                    }

                    // currentAttribute has been filled, handle attribute values now
                    else
                    {
                        if (character == '"')
                        {
                            isInAttributeValue = !isInAttributeValue;
                        }
                        else
                        {
                            attributeBuffer.Append(character);
                        }
                    }
                }
            }

            return tagAttributes;
        }


        public MastoContent HandleLinkTag()
        {
            MastoContent contentToReturn = null;

            var tagAttributes = FindAttributes();

            // Now try to take action depending on the result of trying to find the "class" attribute
            bool hasClassAttribute = tagAttributes.ContainsKey(ParserConstants.ClassAttribute);
            bool isUniqueLink = false;

            string classAttributeValue = string.Empty;

            if (hasClassAttribute)
            {
                classAttributeValue = tagAttributes[ParserConstants.ClassAttribute];
                if (classAttributeValue != string.Empty)
                {
                    isUniqueLink = true;
                }
            }


            if (isUniqueLink)
            {
                // handle a mention/hashtag.
                switch (classAttributeValue)
                {
                    case ParserConstants.HashtagClass:
                        contentToReturn = ParseUniqueLink('#');
                        break;
                    case ParserConstants.MentionClass:
                        contentToReturn = ParseUniqueLink('@');
                        break;
                }

            }
            else
            {
                // Do regular link stuff
                contentToReturn = new MastoContent(tagAttributes[ParserConstants.LinkHref], MastoContentType.Link);
                SkipToLinkTagEnd();
            }

            return contentToReturn;
        }



        private MastoContent ParseUniqueLink(char uniqueChar)
        {
            MastoContent contentToReturn = null;
            bool wasUniqueCharFound = false;
            bool linkTagEndReached = false;
            bool spanTagReached = false;
            bool isInSpanTagContent = false;

            StringBuilder uniqueLinkBuffer = new StringBuilder();

            while (!linkTagEndReached)
            {
                char charFound = charQueue.Dequeue();
                if (wasUniqueCharFound == false)
                {
                    if (charFound == uniqueChar)
                    {
                        wasUniqueCharFound = true;
                    }
                }
                else if (!spanTagReached)
                {
                    if (charFound == '<')
                    {
                        spanTagReached = true;
                    }
                }
                else if (isInSpanTagContent)
                {
                    if (charFound == '<')
                    {
                        // Save buffer content
                        // clear buffer
                        // dequeue to link tag end
                        // set isLinkTagReached to true
                        string uniqueLinkContent = WebUtility.HtmlDecode(uniqueLinkBuffer.ToString());
                        MastoContentType contentType = determineContentTypeFromChar(uniqueChar);
                        contentToReturn = new MastoContent(uniqueLinkContent, contentType);
                        uniqueLinkBuffer.Clear();
                        SkipToLinkTagEnd();
                        linkTagEndReached = true;
                    }
                    else
                    {
                        uniqueLinkBuffer.Append(charFound);
                    }
                }
                else if (spanTagReached)
                {
                    if (charFound == '>')
                    {
                        isInSpanTagContent = true;
                    }
                }
            }


            return contentToReturn;
        }


        private void SkipToLinkTagEnd()
        {
            StringBuilder stringBuffer = new StringBuilder();
            while (!stringBuffer.ToString().Contains($"</{ParserConstants.LinkTag}>"))
            {
                stringBuffer.Append(charQueue.Dequeue());
            }
        }

        private MastoContentType determineContentTypeFromChar(char uniqueChar)
        {
            if (uniqueChar == '#')
            {
                return MastoContentType.Hashtag;
            }
            else
            {
                return MastoContentType.Mention;
            }
        }
    }
}

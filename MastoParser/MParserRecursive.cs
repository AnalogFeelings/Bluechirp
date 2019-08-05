using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MastoParser
{
    public class MParserRecursive
    {

        StringBuilder _parseBuffer = new StringBuilder();
        Queue<char> charQueue = new Queue<char>();
        const char SpaceChar = (char)20;


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
                        parsedContent.Add(new MastoText(textHandlingResult.text));
                    }

                    if (textHandlingResult.hasTagToParse)
                    {
                        parsedTag = textHandlingResult.tag;
                    }
                }
            }

            parsedContent.Add(new MastoText(_parseBuffer.ToString()));
            return parsedContent;
        }

        private bool checkIfLinkTag(string parsedTag)
        {
            return parsedTag.Equals(ParserConstants.LinkTag);
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
                willLoopContinue = parsedTag == $"/{tag}>";
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
                text = _parseBuffer.ToString();
                _parseBuffer.Clear();

                hasTextToParse = true;
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
            while (charQueue.Peek() != SpaceChar)
            {
                parsedTagBuffer.Append(charQueue.Dequeue());
            }
            return parsedTagBuffer.ToString();
        }

        private (bool wasAttributeFound, KeyValuePair<string, string> attributeToAdd) FindAttributes(char character, string currentAttribute = "", string attributeValue = "", bool inAttributeValue = false)
        {
            bool wasAttributeFound = false;
            KeyValuePair<string, string> attributeToAdd = new KeyValuePair<string, string>(null, null);
            if (character == ParserConstants.AttributeCharacter)
            {
                currentAttribute = _parseBuffer.ToString().Trim();
                _parseBuffer.Clear();
            }
            else if (character == ParserConstants.AttributeValueCharacter && !inAttributeValue)
            {
                inAttributeValue = true;
            }
            else if (character == ParserConstants.AttributeValueCharacter && inAttributeValue)
            {
                attributeToAdd = new KeyValuePair<string, string>(currentAttribute, _parseBuffer.ToString());

                _parseBuffer.Clear();
                inAttributeValue = false;
            }
            else
            {
                _parseBuffer.Append(character);
            }

            return (wasAttributeFound, attributeToAdd);
        }

        private void HandleNewTag(string currentTag, ref bool inBreakTag)
        {
            if (currentTag == ParserConstants.BreakTag)
            {
                inBreakTag = true;
            }
        }

        public MastoContent HandleLinkTag()
        {
            MastoContent contentToReturn = null;

            // 1. Look for attributes
            // 2. Based on class attribute, handle links differently.


            // Links may be:
            // 1. Mentions
            // 2. Hashtags
            // 3. Links (just an address to a website somewhere)



            StringBuilder attributeBuffer = new StringBuilder();
            string currentAttribute = "";
            bool hasTagBeenClosed = false;
            bool isInAttributeValue = false;
            Dictionary<string, string> tagAttributes = new Dictionary<string, string>();


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

            // Now take action depending on the result of trying to find the "class" attribute

            bool isRegularLink = tagAttributes.ContainsKey(ParserConstants.ClassAttribute);

            if (isRegularLink)
            {
                // Do regular link stuff
                contentToReturn = new MastoContent(tagAttributes[ParserConstants.LinkHref], MastoContentType.Link);
            }
            else
            {
                // handle a mention/hashtag.

                switch (tagAttributes[ParserConstants.ClassAttribute])
                {
                    case ParserConstants.HashtagClass:
                        contentToReturn = ParseUniqueLink('#');
                        break;
                    case ParserConstants.MentionClass:
                        contentToReturn = ParseUniqueLink('@');
                        break;
                }
            }

            return contentToReturn;
        }
       

        private MastoContent ParseUniqueLink(char uniqueChar)
        {
            MastoContent contentToReturn = null;
            bool betweenTags = true;
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
                else if (spanTagReached)
                {
                    if (charFound == '>')
                    {
                        isInSpanTagContent = true;
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
                        string uniqueLinkContent = uniqueLinkBuffer.ToString();
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

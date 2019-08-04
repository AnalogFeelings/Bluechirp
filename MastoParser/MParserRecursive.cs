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


        public List<MastoContent> ParseLoop(string tag)
        {
            bool inAttributeValue = false;
            bool isInTag = false;
            bool isTagOpen = false;
            string currentAttribute = "";

            string parsedTag = String.Empty;
            bool inBreakTag = false;


            Dictionary<string, string> currentTagAttributes = new Dictionary<string, string>();

            List<MastoContent> parsedContent = new List<MastoContent>();

            while (charQueue.Count > 0)
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

                else if (isInTag)
                {
                    if (isTagOpen)
                    {

                        // Will find attributes here of tag found (should leave this till last):

                        //var attributesSearchResult = FindAttributes(character);
                        //if (attributesSearchResult.wasAttributeFound)
                        //{

                        //}
                    }
                    else
                    {
                        // Parse through tag inside tag
                        //var recursiveContent = ParseLoop();
                        parsedContent.AddRange(recursiveContent);
                    }
                }
                else
                {
                    // Parse through inner content between open and close tags
                    var textHandlingResult = HandleText(character, ref isInTag, ref inBreakTag);
                    if (textHandlingResult.hasContentToParse)
                    {
                        parsedContent.Add(textHandlingResult.contentToParse);
                    }

                }
            }
            return parsedContent;
        }

        private bool CheckIfBreakTag(string currentTag)
        {
            return currentTag.Equals(ParserConstants.BreakTag);
        }

        public List<MastoContent> ParseContent(string htmlContent)
        {
            charQueue = new Queue<char>(htmlContent);

            // At first, you'll start with no tag
            List<MastoContent> parsedContent = ParseLoop();

            return parsedContent;
        }

        private (bool hasTextToParse, bool hasTagToParse, string text, string tag) HandleText(char character)
        {
            bool hasTextToParse = false;
            bool hasTagToParse = null;
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
            // Remember to add '<' has been removed from queue already.
            StringBuilder parsedTagBuffer = new StringBuilder();

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

        public MastoContent HandleLinkTag(string htmlContent)
        {
            MastoContent contentToReturn = null;

            // 1. look for class attribute
            // 2. Based on class attribute, handle links differently.


            // Links may be:
            // 1. Mentions
            // 2. Hashtags
            // 3. Links (just an address to a website somewhere)




            return contentToReturn;
        }
    }
}

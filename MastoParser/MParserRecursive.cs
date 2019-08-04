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


        public List<MastoContent> ParseLoop(string currentTag)
        {
            bool inAttributeValue = false;
            bool inTag = false;
            bool inBreakTag = false;
            bool isTagOpen = false;
            string currentAttribute = "";
            Dictionary<string, string> currentTagAttributes = new Dictionary<string, string>();

            List<MastoContent> parsedContent = new List<MastoContent>();

            while(charStack.Count > 0)
            {
                htmlContent = htmlContent.Substring(1);
                if (inBreakTag)
                {
                    if (character != ParserConstants.TagEndCharacter)
                    {
                        continue;
                    }
                    else
                    {
                        inBreakTag = false;
                    }
                }

                if (currentTag != string.Empty)
                {
                    if (isTagOpen)
                    {
                        FindAttributes(character);

                    }
                    else
                    {
                        var recursiveContent = ParseContent(currentTag);
                    }
                }
                else
                {
                    var textHandlingResult = HandleText(character, ref inTag);
                    if (textHandlingResult.hasContentToParse)
                    {
                        parsedContent.Add(textHandlingResult.contentToParse);
                    }

                }
            }
            return parsedContent;
        }

        public List<MastoContent> ParseContent(string htmlContent)
        {
            charQueue = new Queue<char>(htmlContent);

            // At first, you'll start with no tag
            List<MastoContent> parsedContent = ParseLoop(String.Empty);

            return parsedContent;
        }

        private (bool hasContentToParse, MastoText contentToParse) HandleText(char character, ref bool inTag, ref bool inBreakTag)
        {
            bool hasContentToParse = false;
            MastoText contentToParse = null;

            if (character == ParserConstants.TagStartCharacter)
            {
                string oldContent = _parseBuffer.ToString();
                contentToParse = new MastoText(oldContent);
                _parseBuffer.Clear();

                hasContentToParse = true;
                inTag = true;
            }

            if (inTag)
            {
                if (char.IsWhiteSpace(character))
                {
                    // This way, the start tag character can be removed
                    // and the current tag can be stored in one line.
                    string newTag = _parseBuffer.Remove(0, 1).ToString().Trim();
                    _parseBuffer.Clear();
                    HandleNewTag(newTag, ref inBreakTag);
                }
            }
            _parseBuffer.Append(character);

            return (hasContentToParse, contentToParse);
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

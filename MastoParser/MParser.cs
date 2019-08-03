using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MastoParser
{
    public class MParser
    {
        StringBuilder _parseBuffer = new StringBuilder();
        string currentTag = "";
        bool inAttributeValue = false;
        bool inTag = false;
        bool inBreakTag = false;
        bool isTagOpen = false;
        string currentAttribute = "";
        Dictionary<string, string> currentTagAttributes = new Dictionary<string, string>();


        public List<MastoContent> ParseContent(string htmlContent)
        {
            List<MastoContent> parsedContent = new List<MastoContent>();

            foreach (var character in htmlContent)
            {

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

                    }
                }
                else
                {
                    var textHandlingResult = HandleText(character);
                    if (textHandlingResult.hasContentToParse)
                    {
                        parsedContent.Add(textHandlingResult.contentToParse);
                    }

                }
            }


            return parsedContent;
        }

        private (bool hasContentToParse, MastoText contentToParse) HandleText(char character)
        {
            bool hasContentToParse = false;
            MastoText contentToParse = null;

            if (character == ParserConstants.TagStartCharacter)
            {
                string oldContent = _parseBuffer.ToString();
                contentToParse = new MastoText(oldContent));
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
                    currentTag = _parseBuffer.Remove(0, 1).ToString().Trim();
                    _parseBuffer.Clear();
                    HandleNewTag();
                }
            }
            _parseBuffer.Append(character);

            return (hasContentToParse, contentToParse);
        }

        private void FindAttributes(char character)
        {
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
                currentTagAttributes[currentAttribute] = _parseBuffer.ToString();
                _parseBuffer.Clear();
                inAttributeValue = false;
            }
            else
            {
                _parseBuffer.Append(character);
            }
        }

        private void HandleNewTag()
        {
            if (currentTag == ParserConstants.BreakTag)
            {
                inBreakTag = true;
            }
        }
    }
}

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


                }
                else
                {
                    if (character == ParserConstants.TagStartCharacter)
                    {
                        string oldContent = _parseBuffer.ToString();
                        parsedContent.Add(new MastoText(oldContent));
                        _parseBuffer.Clear();
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
                }
            }


            return parsedContent;
        }

        private void HandleNewTag()
        {
            if(currentTag == ParserConstants.BreakTag)
            {
                inBreakTag = true;
            }
        }
    }
}

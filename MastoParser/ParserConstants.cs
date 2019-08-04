using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MastoParser
{
    class ParserConstants
    {
        public const char ParagraphTag = 'p';
        public const char LinkTag = 'a';
        public const char TagStartCharacter = '<';
        public const char TagEndCharacter = '>';
        public const char TagCloseCharaacter = '/';
        public const char AttributeValueCharacter = '"';
        public const char AttributeCharacter = '=';

        public const string BreakTag = "br";
        public const string SpanTag = "span";

        public const string MentionClass = "u-url mention";
        public const string HashtagClass = "mention hashtag";
        public const string ClassAttribute = "class";

    }
}

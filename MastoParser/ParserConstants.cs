using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MastoParser
{
    class ParserConstants
    {
        const char ParagraphTag = 'p';
        const char LinkTag = 'a';
        const char TagStartCharacter = '<';
        const char TagEndCharacter = '>';
        const char TagCloseCharaacter = '/';
        const char AttributeValueCharacter = '"';
        const char AttributeCharacter = '=';

        const string BreakTag = "br";
        const string SpanTag = "span";

    }
}

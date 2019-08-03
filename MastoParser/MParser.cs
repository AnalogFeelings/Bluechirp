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


        public List<MastoContent> ParseContent(string htmlContent)
        {
            List<MastoContent> parsedContent = new List<MastoContent>();




            return parsedContent;
        }
    }
}

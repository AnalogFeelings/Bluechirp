using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MastoParser
{
    public class MastoText
    {
        public bool IsParagraph { get; set; }
        public string Content { get; set; }

        public MastoText(string content, bool isParagraph = false)
        {
            Content = content;
            IsParagraph = isParagraph;
        }
    }
}

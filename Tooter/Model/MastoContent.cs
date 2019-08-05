using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tooter.Model
{
    public class MastoContent
    {
        public string Content { get; set; }
        public MastoContentType ContentType { get; set; }

        public MastoContent(string content, MastoContentType contentType)
        {
            Content = content;
            ContentType = contentType;
        }
    }
}

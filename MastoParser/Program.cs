using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MastoParser
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {

                Console.WriteLine("Enter content to parse:");
                string contentToParse = Console.ReadLine();

                // Parse content
                MParserRecursive parser = new MParserRecursive();
                List<MastoContent> parsedContent = parser.ParseContent(contentToParse);

                foreach (var item in parsedContent)
                {
                    Console.Write(item.ContentType + ": ");
                    Console.WriteLine(item.Content + "\n");
                }
               

            }

        }
    }
}

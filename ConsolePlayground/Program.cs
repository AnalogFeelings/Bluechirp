using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            string mahString = "abeeceedee";
            Stack<char> charStack = new Stack<char>(mahString.Reverse());
            foreach (var item in charStack)
            {
                Console.WriteLine(item);
            }
            Console.ReadLine();
        }
    }
}


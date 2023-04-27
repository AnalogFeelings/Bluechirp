using System;
using System.Text;
using System.Text.RegularExpressions;
using MastoParserLib;

namespace MastoConsolePlayground
{
    class Program
    {
        const string InstanceNameRegularExpression = "^[A-Za-z0-9\\-]+\\.+[A-Za-z0-9\\-]+$";
        const string wwwString = "www.";
        static void Main(string[] args)
        {


            const string exampleUrl = "https://www.masto-don.pho/";
            checkIfInstanceFormat(exampleUrl);


            string finalUrl = "";

            var protocolSplit = exampleUrl.Split("://");



            if (CheckIfSplitCanContinue(protocolSplit))
            {

                string noProtocolString = protocolSplit[1];
                if (noProtocolString.StartsWith(wwwString))
                {
                    noProtocolString = noProtocolString.Remove(0, wwwString.Length);
                }

                string finalTreatmentString = TreatFinalSplit(noProtocolString);
                checkIfInstanceFormat(finalTreatmentString);

            }
            else
            {
                finalUrl = exampleUrl;
            }
            //var leftPartSplit = exampleUrl.Split("https://");
            //var goodPart = leftPartSplit[1].Split("www.");
            Console.WriteLine(finalUrl);
        }

        private static void checkIfInstanceFormat(string treatedString)
        {
            // Define a regular expression for repeated words.
            Regex rx = new Regex(InstanceNameRegularExpression,
              RegexOptions.Compiled | RegexOptions.IgnoreCase);



            // Find matches.
            MatchCollection matches = rx.Matches(treatedString);

            // Report the number of matches found.
            Console.WriteLine("{0} matches found in:\n   {1}",
                              matches.Count,
                              treatedString);

            // Report on each match.
            foreach (Match match in matches)
            {
                GroupCollection groups = match.Groups;
                Console.WriteLine("'{0}' repeated at positions {1} and {2}",
                                  groups["word"].Value,
                                  groups[0].Index,
                                  groups[1].Index);
            }
        }

        private static string TreatFinalSplit(string v)
        {
            v = v.Replace("/", "");
            v = v.Replace("\\", "");
            return v;
        }

        private static bool CheckIfSplitCanContinue(string[] splitArray)
        {
            bool canContinue = false;

            if (splitArray.Length > 1)
            {
                if (splitArray[1].Length > 0)
                {
                    canContinue = true;
                }
            }
            return canContinue;
        }
    }
}

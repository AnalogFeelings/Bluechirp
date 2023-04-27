using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluechirpLib.Helpers
{
    public class CharCounterHelper
    {
        public static (int charactersFound, bool characterLimitExceeded) CountCharactersWithLimit(string charactersToCount, int characterLimit)
        {
            int charactersFound = charactersToCount.Length;
            bool characterLimitExceeded = charactersFound > characterLimit;

            return (charactersFound, characterLimitExceeded);

        }
    }
}

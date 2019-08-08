using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tooter.Helpers
{
    public class CharCounterHelper
    {
        public (int charactersFound, bool characterLimitReached) CountCharactersWithLimit(string charactersToCount, int characterLimit)
        {
            int charactersFound = charactersToCount.Length;
            bool characterLimitReached = charactersFound > characterLimit;

            return (charactersFound, characterLimitReached);

        }
    }
}

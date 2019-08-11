using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tooter.Helpers
{
    public static class TimestampHelper
    {
        const char YearStamp = 'y';
        const char MonthStamp = 'm';
        const char DayStamp = 'd';
        const char MinuteStamp = 'm';
        const char SecondStamp = 's';
        const string NowStamp = "Now";
        const int DaysInYear = 365;
        const int DaysInAMonth = 30;

        public static string FormatTimestamp(DateTime timestampToFormat)
        {
            var timeBetweenPostAndNow = CalculateTimeBetweenPostAndPresent(timestampToFormat);
            
            StringBuilder timestampBuilder = new StringBuilder();
            if (timeBetweenPostAndNow.Days >= DaysInYear)
            {
                int numOfYears = timeBetweenPostAndNow.Days / 365;
                timestampBuilder.Append($"{numOfYears}{YearStamp}");
            }
            else if(timeBetweenPostAndNow.Days > DaysInAMonth)
            {
                int numOfMonths = timeBetweenPostAndNow.Days / DaysInAMonth;
                timestampBuilder.Append($"{numOfMonths}{MonthStamp}");
            }
            else if (timeBetweenPostAndNow.Days > 0)
            {
                timestampBuilder.Append($"{timeBetweenPostAndNow.Days}{DayStamp}");
            }
            else if (timeBetweenPostAndNow.Minutes > 0)
            {
                timestampBuilder.Append($"{timeBetweenPostAndNow.Minutes}{MinuteStamp}");
            }
            else if (timeBetweenPostAndNow.Seconds > 0)
            {
                timestampBuilder.Append($"{timeBetweenPostAndNow.Seconds}{SecondStamp}");
            }
            else
            {
                timestampBuilder.Append(NowStamp);
            }

            return timestampBuilder.ToString();
        }

        private static TimeSpan CalculateTimeBetweenPostAndPresent(DateTime timestampToFormat)
        {
            var currentUniversalDateTime = DateTime.UtcNow;
            var postUniversalDateTime = timestampToFormat.ToUniversalTime();

            var timeLeft = currentUniversalDateTime - postUniversalDateTime;
            return timeLeft;
        }
    }
}

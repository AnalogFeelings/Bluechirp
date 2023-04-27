using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooterLib.Helpers
{
    public static class TimestampHelper
    {
        const char YearStamp = 'y';
        const char MonthStamp = 'm';
        const char DayStamp = 'd';
        const char HourStamp = 'h';
        const char MinuteStamp = 'm';
        const char SecondStamp = 's';
        const string NowStamp = "Now";
        const int DaysInYear = 365;
        const int DaysInAMonth = 30;

        public static string FormatTimestamp(DateTime timestampToFormat)
        {
            var timeBetweenPostAndNow = CalculateTimeBetweenPostAndPresent(timestampToFormat);
            
            StringBuilder timestampBuilder = new StringBuilder();
            if (timeBetweenPostAndNow.TotalDays >= DaysInYear)
            {
                int numOfYears = timeBetweenPostAndNow.Days / 365;
                timestampBuilder.Append($"{numOfYears}{YearStamp}");
            }
            else if(timeBetweenPostAndNow.TotalDays > DaysInAMonth)
            {
                int numOfMonths = (int)timeBetweenPostAndNow.TotalDays / DaysInAMonth;
                timestampBuilder.Append($"{numOfMonths}{MonthStamp}");
            }
            else if (timeBetweenPostAndNow.TotalDays >= 1)
            {
                timestampBuilder.Append($"{(int)timeBetweenPostAndNow.TotalDays}{DayStamp}");
            }
            else if (timeBetweenPostAndNow.TotalHours >= 1)
            {
                timestampBuilder.Append($"{(int)timeBetweenPostAndNow.TotalHours}{HourStamp}");
            }
            else if (timeBetweenPostAndNow.TotalMinutes >= 1)
            {
                timestampBuilder.Append($"{(int)timeBetweenPostAndNow.TotalMinutes}{MinuteStamp}");
            }
            else if (timeBetweenPostAndNow.TotalSeconds >= 1)
            {
                timestampBuilder.Append($"{(int)timeBetweenPostAndNow.TotalSeconds}{SecondStamp}");
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

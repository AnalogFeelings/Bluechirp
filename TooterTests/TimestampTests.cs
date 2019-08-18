using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooterLib.Helpers;

namespace TooterTests
{
    [TestClass]
    public class TimestampTests
    {
        [TestMethod]
        public void TestIfHourFormattedCorrectly()
        {
            const string expectedTimestamp = "1h";
            DateTime pastTime = DateTime.Now.Subtract(new TimeSpan(1, 0, 0));

            string formattedTimestamp = TimestampHelper.FormatTimestamp(pastTime);
            Assert.IsTrue(expectedTimestamp == formattedTimestamp);
        }

        [TestMethod]
        public void TestIfMinsFormattedCorrectly()
        {
            const string expectedTimestamp = "2m";
            DateTime pastTime = DateTime.Now.Subtract(new TimeSpan(0, 2, 0));

            string formattedTimestamp = TimestampHelper.FormatTimestamp(pastTime);
            Assert.IsTrue(expectedTimestamp == formattedTimestamp);
        }

        [TestMethod]
        public void TestIfSecondsFormattedCorrectly()
        {
            const string expectedTimestamp = "4s";
            DateTime pastTime = DateTime.Now.Subtract(new TimeSpan(0, 0, 4));

            string formattedTimestamp = TimestampHelper.FormatTimestamp(pastTime);
            Assert.IsTrue(expectedTimestamp == formattedTimestamp);
        }

        [TestMethod]
        public void TestIfDaysFormattedCorrectly()
        {
            const string expectedTimestamp = "7d";
            DateTime pastTime = DateTime.Now.Subtract(new TimeSpan(7,0, 0, 0));

            string formattedTimestamp = TimestampHelper.FormatTimestamp(pastTime);
            Assert.IsTrue(expectedTimestamp == formattedTimestamp);
        }

        [TestMethod]
        public void TestIfMonthsFormattedCorrectly()
        {
            const string expectedTimestamp = "6m";

            // Average month is 30 days but in reality, month days vary
            const string otherExpectedValue = "5m";
            const int daysInMonth = 30;
            DateTime pastTime = DateTime.Now.Subtract(new TimeSpan(daysInMonth * 6, 0, 0, 0));

            string formattedTimestamp = TimestampHelper.FormatTimestamp(pastTime);
            Assert.IsTrue(expectedTimestamp == formattedTimestamp || otherExpectedValue == formattedTimestamp);
        }

        [TestMethod]
        public void TestIfYearsFormattedCorrectly()
        {
            const string expectedTimestamp = "1y";
            
            // Sometimes you have leap years
            const string otherExpectedValue = "11m";
            

            const int daysInYear = 365;
            
            DateTime pastTime = DateTime.Now.Subtract(new TimeSpan(daysInYear, 0, 0, 0));

            string formattedTimestamp = TimestampHelper.FormatTimestamp(pastTime);
            Assert.IsTrue(expectedTimestamp == formattedTimestamp || otherExpectedValue == formattedTimestamp);
        }
    }
}

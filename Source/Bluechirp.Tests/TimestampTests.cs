using System;
using Bluechirp.Library.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bluechirp.Tests
{
    [TestClass]
    public class TimestampTests
    {
        [TestMethod]
        public void TestIfHourFormattedCorrectly()
        {
            string expectedTimestamp = "1h";
            DateTime pastTime = DateTime.Now.Subtract(new TimeSpan(1, 0, 0));
            string formattedTimestamp = TimestampHelper.FormatTimestamp(pastTime);

            Assert.AreEqual(expectedTimestamp, formattedTimestamp);
        }

        [TestMethod]
        public void TestIfMinsFormattedCorrectly()
        {
            string expectedTimestamp = "2m";
            DateTime pastTime = DateTime.Now.Subtract(new TimeSpan(0, 2, 0));
            string formattedTimestamp = TimestampHelper.FormatTimestamp(pastTime);

            Assert.AreEqual(expectedTimestamp, formattedTimestamp);
        }

        [TestMethod]
        public void TestIfSecondsFormattedCorrectly()
        {
            string expectedTimestamp = "4s";
            DateTime pastTime = DateTime.Now.Subtract(new TimeSpan(0, 0, 4));
            string formattedTimestamp = TimestampHelper.FormatTimestamp(pastTime);

            Assert.AreEqual(expectedTimestamp, formattedTimestamp);
        }

        [TestMethod]
        public void TestIfDaysFormattedCorrectly()
        {
            string expectedTimestamp = "7d";
            DateTime pastTime = DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0));
            string formattedTimestamp = TimestampHelper.FormatTimestamp(pastTime);

            Assert.AreEqual(expectedTimestamp, formattedTimestamp);
        }

        [TestMethod]
        public void TestIfMonthsFormattedCorrectly()
        {
            string expectedTimestamp = "6m";
            string otherExpectedValue = "5m"; // Average month is 30 days but in reality, month days vary

            int daysInMonth = 30;

            DateTime pastTime = DateTime.Now.Subtract(new TimeSpan(daysInMonth * 6, 0, 0, 0));
            string formattedTimestamp = TimestampHelper.FormatTimestamp(pastTime);

            Assert.IsTrue(expectedTimestamp == formattedTimestamp || otherExpectedValue == formattedTimestamp);
        }

        [TestMethod]
        public void TestIfYearsFormattedCorrectly()
        {
            string expectedTimestamp = "1y";
            string otherExpectedValue = "11m"; // Sometimes you have leap years

            int daysInYear = 365;

            DateTime pastTime = DateTime.Now.Subtract(new TimeSpan(daysInYear, 0, 0, 0));
            string formattedTimestamp = TimestampHelper.FormatTimestamp(pastTime);

            Assert.IsTrue(expectedTimestamp == formattedTimestamp || otherExpectedValue == formattedTimestamp);
        }
    }
}
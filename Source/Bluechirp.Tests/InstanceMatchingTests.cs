using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bluechirp.Library.Services;

namespace Bluechirp.Tests
{
    [TestClass]
    public class InstanceMatchingTests
    {
        [TestMethod]
        public void TestUrlFailingTest()
        {
            string testUrl = "https://www.google.com";
            Assert.IsFalse(InstanceMatchService.CheckIfInstanceNameIsProperlyFormatted(testUrl));
        }

        [TestMethod]
        public void TestCorrectlyFormattedName()
        {
            string instanceName = "mastodon.technology";
            Assert.IsTrue(InstanceMatchService.CheckIfInstanceNameIsProperlyFormatted(instanceName));
        }
    }
}

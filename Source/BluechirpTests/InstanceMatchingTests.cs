using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BluechirpLib.Services;

namespace BluechirpTests
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

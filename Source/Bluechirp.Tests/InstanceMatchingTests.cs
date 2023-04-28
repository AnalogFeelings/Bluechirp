using Bluechirp.Library.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
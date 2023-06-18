using Bluechirp.Library.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bluechirp.Tests
{
    [TestClass]
    public class InstanceMatchingTests
    {
        private InstanceMatchService instanceMatchService;

        public InstanceMatchingTests() 
        { 
            instanceMatchService = new InstanceMatchService();
        }

        [TestMethod]
        public void TestUrlFailingTest()
        {
            string testUrl = "https://www.google.com";

            Assert.IsFalse(instanceMatchService.CheckIfInstanceNameIsProperlyFormatted(testUrl));
        }

        [TestMethod]
        public void TestCorrectlyFormattedName()
        {
            string instanceName = "mastodon.technology";

            Assert.IsTrue(instanceMatchService.CheckIfInstanceNameIsProperlyFormatted(instanceName));
        }
    }
}
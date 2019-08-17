
using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TooterLib.Core;
using TooterLib.Helpers;

namespace TooterTests
{
    [TestClass]
    public class AuthTests
    {
        [TestMethod]
        public async Task CheckIfLoginWorks()
        {
            string[] testConstants = { "FediTest", "https://www.google.com", "tooter://mycallback" };

            APIConstants.SetConstants(testConstants[0], testConstants[1], testConstants[2]);

            const string testInstance = "mastodon.social";

            try
            {
                await AuthHelper.Instance.CreateOAuthUrlAsync(testInstance);
            }
            catch (Exception)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public async Task 
    }
}

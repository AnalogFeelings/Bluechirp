using System.Threading.Tasks;
using Bluechirp.Library.Helpers;
using Bluechirp.Tests.Generators;
using Mastonet.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bluechirp.Tests
{
    [TestClass]
    public class ClientTests
    {
        [TestMethod]
        public async Task TestProfileLoad()
        {
            string testUserId = "testID";
            Auth token;
            AppRegistration registration;

            (token, registration) = ClientData.CreateFakeClientAuthObjects();

            string testClientProfileId = registration.Instance + testUserId;

            await ClientDataHelper.StoreClientData(testClientProfileId, token, registration);

            // Delay to remove lock on files
            await Task.Delay(1000);

            ClientHelper.LoadProfile(testClientProfileId);
            Assert.AreEqual(token.AccessToken, ClientHelper.Client.AccessToken);
            Assert.AreEqual(testClientProfileId, ClientHelper.LoadedProfile);
        }

        [TestMethod]
        public async Task TestLastUsedProfileLoad()
        {
            string testUserId = "testID";
            Auth token;
            AppRegistration registration;

            (token, registration) = ClientData.CreateFakeClientAuthObjects();

            string testClientProfileId = registration.Instance + testUserId;

            await ClientDataHelper.StoreClientData(testClientProfileId, token, registration);

            ClientDataHelper.SetLastUsedProfile(testClientProfileId);

            // Delay to remove lock on files
            await Task.Delay(1000);

            Assert.IsTrue(ClientHelper.LoadLastUsedProfile());
        }

        [TestMethod]
        public void TestSetLastUsedProfile()
        {
            string profileToSet = "test";

            ClientHelper.SetLoadedProfile(profileToSet);
            Assert.AreEqual(profileToSet, ClientHelper.LoadedProfile);
        }
    }
}
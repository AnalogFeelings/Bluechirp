using Mastonet.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bluechirp.Library.Helpers;
using Bluechirp.Tests.Generators;

namespace Bluechirp.Tests
{
    [TestClass]
    public class ClientTests
    {
        [TestMethod]
        public async Task TestProfileLoad()
        {
            string testUserID = "testID";
            Auth token;
            AppRegistration registration;

            (token, registration) = ClientData.CreateFakeClientAuthObjects();

            string testClientProfileID = registration.Instance + testUserID;

            await ClientDataHelper.StoreClientData(testClientProfileID, token, registration);

            // Delay to remove lock on files
            await Task.Delay(1000);

            ClientHelper.LoadProfile(testClientProfileID);
            Assert.IsTrue(ClientHelper.Client.AccessToken == token.AccessToken);
            Assert.IsTrue(ClientHelper.LoadedProfile == testClientProfileID);
        }

        [TestMethod]
        public async Task TestLastUsedProfileLoad()
        {

            string testUserID = "testID";
            Auth token;
            AppRegistration registration;

            (token, registration) = ClientData.CreateFakeClientAuthObjects();

            string testClientProfileID = registration.Instance + testUserID;

            await ClientDataHelper.StoreClientData(testClientProfileID, token, registration);

            ClientDataHelper.SetLastUsedProfile(testClientProfileID);

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

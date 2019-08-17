using Mastonet.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooterLib.Helpers;
using TooterTests.Generators;

namespace TooterTests
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
            Assert.IsTrue(ClientHelper.Client.AppRegistration.ClientId == registration.ClientId);
            Assert.IsTrue(ClientHelper.Client.AuthToken.AccessToken == token.AccessToken);
            Assert.IsTrue(ClientHelper.LoadedProfile == testClientProfileID);
        }
    }
}

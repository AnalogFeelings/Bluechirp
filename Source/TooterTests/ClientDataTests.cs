using Mastonet.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooterLib.Enums;
using TooterLib.Helpers;
using TooterLib.Model;
using TooterTests.Generators;
using Windows.Storage;

namespace TooterTests
{
    [TestClass]
    public class ClientDataTests
    {
        [TestMethod]
        public async Task TestCacheFileIO()
        {
            var timelineType = TimelineType.Home;
            var fakeTimelineCache = TimelineData.GenerateTimelineCache(timelineType);

            try
            {
                await ClientDataHelper.StoreTimelineCacheAsync(fakeTimelineCache);
            }

            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            // In order to allow windows to release lock on file
            // to allow load
            await Task.Delay(2000);

            var loadResult = await ClientDataHelper.LoadTimelineFromFileAsync(timelineType);
            if (loadResult.wasTimelineLoaded)
            {
                Assert.IsTrue(fakeTimelineCache.Toots[0].Content == loadResult.cacheToReturn.Toots[0].Content);
            }
            else
            {
                Assert.Fail("Cache was not loaded");
            }
        }

        [TestMethod]
        public async Task TestCacheDeletion()
        {
            var timelineType = TimelineType.Home;
            var fakeTimelineCache = TimelineData.GenerateTimelineCache(timelineType);

            try
            {
                await ClientDataHelper.StoreTimelineCacheAsync(fakeTimelineCache);
            }

            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            // In order to allow windows to release lock on file
            // to allow load
            await Task.Delay(2000);

            try
            {
                await ClientDataHelper.ClearTimelineCache();
            }
            catch (Exception ex)
            {
                // Should never fail
                Assert.Fail(ex.Message);
            }

            // Need to manually check if there are any cache files left
            // to prove that cache has actually been cleared
            var cacheFolderContents = await ApplicationData.Current.TemporaryFolder.GetFilesAsync();
            Assert.IsFalse(cacheFolderContents.Count > 0);

        }

        [TestMethod]
        public async Task ClientDataStartupTest()
        {
            try
            {
                await ClientDataHelper.StartUpAsync();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod]
        public async Task TestClientDataStorage()
        {
            string testUserID = "testID";
            Auth token;
            AppRegistration registration;

            (token, registration) = ClientData.CreateFakeClientAuthObjects();

            string testClientProfileID = registration.Instance + testUserID;

            await ClientDataHelper.StoreClientData(testClientProfileID, token, registration);

            // Delay to remove lock on files
            await Task.Delay(1000);

            Auth loadedToken;
            AppRegistration loadedRegistration;
            (loadedRegistration, loadedToken) = ClientDataHelper.LoadClientProfile(testClientProfileID);

            Assert.IsTrue(loadedToken.AccessToken == token.AccessToken);
            Assert.AreEqual(loadedRegistration.ClientId, registration.ClientId);

        }

        [TestMethod]
        public async Task TestClientDataDeletion()
        {
            string testUserID = "testID";
            Auth token;
            AppRegistration registration;

            (token, registration) = ClientData.CreateFakeClientAuthObjects();

            string testClientProfileID = registration.Instance + testUserID;

            await ClientDataHelper.StoreClientData(testClientProfileID, token, registration);

            // Delay to remove lock on files
            await Task.Delay(1000);

            await ClientDataHelper.RemoveClientProfileAsync(testClientProfileID);

            // Need to check if local settings container has really been deleted

            var localContainers = ApplicationData.Current.LocalSettings.Containers;
            Assert.IsFalse(localContainers.ContainsKey(testClientProfileID));
        }



    }
}

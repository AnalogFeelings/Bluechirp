using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Bluechirp.Library.Enums;
using Bluechirp.Library.Helpers;
using Bluechirp.Library.Model;
using Bluechirp.Tests.Generators;
using Mastonet.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bluechirp.Tests
{
    [TestClass]
    public class ClientDataTests
    {
        [TestMethod]
        public async Task TestCacheFileIO()
        {
            TimelineType timelineType = TimelineType.Home;
            TimelineCache fakeTimelineCache = TimelineData.GenerateTimelineCache(timelineType);

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

            (bool wasTimelineLoaded, TimelineCache cacheToReturn) loadResult = await ClientDataHelper.LoadTimelineFromFileAsync(timelineType);
            if (loadResult.wasTimelineLoaded)
                Assert.AreEqual(fakeTimelineCache.Toots[0].Content, loadResult.cacheToReturn.Toots[0].Content);
            else
                Assert.Fail("Cache was not loaded correctly.");
        }

        [TestMethod]
        public async Task TestCacheDeletion()
        {
            TimelineType timelineType = TimelineType.Home;
            TimelineCache fakeTimelineCache = TimelineData.GenerateTimelineCache(timelineType);

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
            IReadOnlyList<StorageFile> cacheFolderContents = await ApplicationData.Current.TemporaryFolder.GetFilesAsync();
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
            string testUserId = "testID";
            Auth token;
            AppRegistration registration;

            (token, registration) = ClientData.CreateFakeClientAuthObjects();

            string testClientProfileId = registration.Instance + testUserId;

            await ClientDataHelper.StoreClientData(testClientProfileId, token, registration);

            // Delay to remove lock on files
            await Task.Delay(1000);

            Auth loadedToken;
            AppRegistration loadedRegistration;
            (loadedRegistration, loadedToken) = ClientDataHelper.LoadClientProfile(testClientProfileId);

            Assert.AreEqual(token.AccessToken, loadedToken.AccessToken);
            Assert.AreEqual(registration.ClientId, loadedRegistration.ClientId);
        }

        [TestMethod]
        public async Task TestClientDataDeletion()
        {
            string testUserId = "testID";
            Auth token;
            AppRegistration registration;

            (token, registration) = ClientData.CreateFakeClientAuthObjects();

            string testClientProfileId = registration.Instance + testUserId;

            await ClientDataHelper.StoreClientData(testClientProfileId, token, registration);

            // Delay to remove lock on files
            await Task.Delay(1000);

            await ClientDataHelper.RemoveClientProfileAsync(testClientProfileId);

            // Need to check if local settings container has really been deleted
            IReadOnlyDictionary<string, ApplicationDataContainer> localContainers = ApplicationData.Current.LocalSettings.Containers;

            Assert.IsFalse(localContainers.ContainsKey(testClientProfileId));
        }
    }
}
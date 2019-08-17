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
                Assert.IsTrue(fakeTimelineCache.Toots.SequenceEqual(loadResult.cacheToReturn.Toots));
            }
            else
            {
                Assert.Fail("Cache was not loaded");
            }
        }

       
        }
    }

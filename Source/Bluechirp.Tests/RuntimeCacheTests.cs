using Bluechirp.Library.Enums;
using Bluechirp.Library.Model;
using Bluechirp.Library.Services;
using Bluechirp.Tests.Generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bluechirp.Tests
{
    [TestClass]
    public class RuntimeCacheTests
    {
        [TestMethod]
        public void CacheTest()
        {
            string generatedContent = "hii";
            TimelineCache cacheToStore = TimelineData.GenerateTimelineCache(TimelineType.Home);

            RuntimeCacheService.StoreCache(cacheToStore, cacheToStore.CurrentTimelineSettings.CurrentTimelineType);

            (bool isCacheAvailable, TimelineCache cache) retreivedTimelineResult = RuntimeCacheService.RetreiveCache(TimelineType.Home);

            Assert.IsTrue(retreivedTimelineResult.isCacheAvailable);
            Assert.AreEqual(generatedContent, retreivedTimelineResult.cache.Toots[0].Content);
        }

        [TestMethod]
        public void CacheClearTest()
        {
            TimelineCache cacheToStore = TimelineData.GenerateTimelineCache(TimelineType.Home);
            RuntimeCacheService.StoreCache(cacheToStore, cacheToStore.CurrentTimelineSettings.CurrentTimelineType);

            RuntimeCacheService.ClearCache(TimelineType.Home);

            (bool isCacheAvailable, TimelineCache cache) retreivedTimelineResult = RuntimeCacheService.RetreiveCache(TimelineType.Home);

            Assert.IsFalse(retreivedTimelineResult.isCacheAvailable);
        }
    }
}
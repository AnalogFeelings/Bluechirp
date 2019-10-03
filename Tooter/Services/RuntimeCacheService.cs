using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooter.Model;
using TooterLib.Enums;
using TooterLib.Model;

namespace Tooter.Services
{
    public class RuntimeCacheService
    {
        static Dictionary<TimelineType, TimelineCacheV2> RuntimeCache = new Dictionary<TimelineType, TimelineCacheV2>();
        
        public static void StoreCache(TimelineCacheV2 cache, TimelineType type)
        {
            RuntimeCache[type] = cache;
        }

        public static void ClearCache(TimelineType type)
        {
            RuntimeCache.Remove(TimelineType.Home);
        }

        public static (bool isCacheAvailable, TimelineCacheV2 cache) RetreiveCache(TimelineType type)
        {
            TimelineCacheV2 cache;
            bool isCacheAvailable = RuntimeCache.TryGetValue(type, out cache);

            return (isCacheAvailable, cache);
        }

       
    }
}

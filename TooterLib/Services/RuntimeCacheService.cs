using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TooterLib.Enums;
using TooterLib.Model;

namespace TooterLib.Services
{
    public class RuntimeCacheService
    {
        static Dictionary<TimelineType, TimelineCache> RuntimeCache = new Dictionary<TimelineType, TimelineCache>();
        //static Dictionary<TimelineType, bool> CacheChangesRegistry = new Dictionary<TimelineType, bool>();
        

        public static void StoreCache(TimelineCache cache, TimelineType type)
        {
            RuntimeCache[type] = cache;
        }

        public static void ClearCache(TimelineType type)
        {
            RuntimeCache[type] = null;
        }

        public static (bool isCacheAvailable, TimelineCache cache) RetreiveCache(TimelineType type)
        {
            TimelineCache cache;
            bool isCacheAvailable = RuntimeCache.TryGetValue(type, out cache);

            return (isCacheAvailable, cache);
        }
    }
}

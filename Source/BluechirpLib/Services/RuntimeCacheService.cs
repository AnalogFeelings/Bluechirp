using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BluechirpLib.Enums;
using BluechirpLib.Model;

namespace BluechirpLib.Services
{
    public class RuntimeCacheService
    {
        static Dictionary<TimelineType, TimelineCache> RuntimeCache = new Dictionary<TimelineType, TimelineCache>();
        
        public static void StoreCache(TimelineCache cache, TimelineType type)
        {
            RuntimeCache[type] = cache;
        }

        public static void ClearCache(TimelineType type)
        {
            RuntimeCache.Remove(TimelineType.Home);
        }

        public static (bool isCacheAvailable, TimelineCache cache) RetreiveCache(TimelineType type)
        {
            TimelineCache cache;
            bool isCacheAvailable = RuntimeCache.TryGetValue(type, out cache);

            return (isCacheAvailable, cache);
        }

       
    }
}

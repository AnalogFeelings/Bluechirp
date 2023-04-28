using System;
using System.Threading.Tasks;
using Windows.Storage;
using Bluechirp.Library.Enums;
using Bluechirp.Library.Helpers;
using Bluechirp.Library.Model;
using Bluechirp.View;
using Mastonet.Entities;

namespace Bluechirp.Services
{
    /// <summary>
    /// Class that handles caching of objects.
    /// </summary>
    public static class CacheService
    {
        private static TimelineView _CurrentTimeline;
        internal static string KeyboardShortuctsContent = "";

        /// <summary>
        /// Stores a timeline inside the app cache.
        /// </summary>
        /// <param name="Timeline">The timeline to store.</param>
        /// <param name="CurrentStatusMarker">I have to figure out what this does.</param>
        /// <param name="TimelineSettings">The timeline settings.</param>
        /// <returns>An awaitable task.</returns>
        internal static async Task CacheTimeline(MastodonList<Status> Timeline, Status CurrentStatusMarker, TimelineSettings TimelineSettings)
        {
            TimelineCache cachedTimeline = new TimelineCache(Timeline, CurrentStatusMarker, TimelineSettings);

            await ClientDataHelper.StoreTimelineCacheAsync(cachedTimeline);
        }

        /// <summary>
        /// Swaps the current timeline with a new one.
        /// </summary>
        /// <param name="NewTimeline">The new timeline.</param>
        internal static void SwapCurrentTimeline(TimelineView NewTimeline)
        {
            _CurrentTimeline = NewTimeline;
        }

        /// <summary>
        /// Stores the current timeline in the app cache.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        internal static async Task CacheCurrentTimeline()
        {
            if (_CurrentTimeline != null) 
                await _CurrentTimeline.TryCacheTimeline();
        }

        /// <summary>
        /// Loads a timeline from the app cache.
        /// </summary>
        /// <param name="TimelineType">The timeline type to load.</param>
        /// <returns>The loaded timeline, and a bool indicating if the load was successful.</returns>
        internal static async Task<(bool wasTimelineLoaded, TimelineCache cacheToReturn)> LoadTimelineCache(TimelineType TimelineType)
        {
            // TODO: Replace these with null checks.
            (bool wasTimelineLoaded, TimelineCache cacheToReturn) cacheLoadResult = await ClientDataHelper.LoadTimelineFromFileAsync(TimelineType);
            bool wasTimelineLoaded = cacheLoadResult.wasTimelineLoaded;
            TimelineCache cacheToReturn = cacheLoadResult.cacheToReturn;

            // Loaded cache from a file is stored into runtime cache to prevent having to load from file repeatedly.
            // Also cache file has been deleted already by this point.
            //if (wasTimelineLoaded)
            //{
            //    RuntimeCacheService.StoreCache(cacheToReturn, cacheToReturn.CurrentTimelineSettings.CurrentTimelineType);
            //}
            return (wasTimelineLoaded, cacheToReturn);
        }

        /// <summary>
        /// Loads the keyboard shortcuts list from the app storage.
        /// </summary>
        /// <returns>An awaitable task.</returns>
        internal static async Task LoadKeyboardShortcutsContent()
        {
            StorageFile shortcutsFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/FediShortcuts.txt"));

            KeyboardShortuctsContent = await FileIO.ReadTextAsync(shortcutsFile);
        }
    }
}
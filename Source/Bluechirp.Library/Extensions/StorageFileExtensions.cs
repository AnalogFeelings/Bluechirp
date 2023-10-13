using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace Bluechirp.Library.Extensions
{
    public static class StorageFileExtensions
    {
        public static async Task<ulong> GetFileSizeAsync(this StorageFile file)
        {
            BasicProperties properties = await file.GetBasicPropertiesAsync();

            return properties.Size;
        }
    }
}

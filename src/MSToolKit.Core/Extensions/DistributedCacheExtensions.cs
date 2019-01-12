using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace MSToolKit.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for using 
    /// over Microsoft.Extensions.Caching.Distributed.IDistributedCache.
    /// </summary>
    public static class DistributedCacheExtensions
    {
        /// <summary>
        /// Sets a specified key and value(serialized to JSON) to the application cache.
        /// </summary>
        /// <param name="cache">The provided instance of IDistributedCache.</param>
        /// <param name="key">The key, that should be stored in the cache.</param>
        /// <param name="value">The value for the given key, that should be stored in the cache.</param>
        /// <param name="expiration">The expiration period of the currently stored KeyValuePair.</param>
        /// <returns>
        /// Microsoft.Extensions.Caching.Distributed.IDistributedCache
        /// </returns>
        public static async Task<IDistributedCache> SetSerializableObject(
            this IDistributedCache cache, string key, object value, TimeSpan expiration)
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration
            };

            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var serializedObject = JsonConvert.SerializeObject(value, jsonSettings);

            await cache.SetStringAsync(key, serializedObject, cacheOptions);

            return cache;
        }

        /// <summary>
        /// Gets a specified object from the cache by a given key.
        /// </summary>
        /// <typeparam name="T">Type of specified serializeable object.</typeparam>
        /// <param name="cache">The provided instance of IDistributedCache.</param>
        /// <param name="key">The key, that should be search for in the cache.</param>
        /// <returns>
        /// An instance of T, if found any for the given key.
        /// </returns>
        public static async Task<T> GetSerializableObject<T>(this IDistributedCache cache, string key)
        {
            var value = await cache.GetStringAsync(key);
            var instance = JsonConvert.DeserializeObject<T>(value);

            return instance;
        }
    }
}

using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace WebAppCoreBlazorServer.Common.Utils
{
    public class RedisUtils
    {
        public static void SetCacheData(IDistributedCache distributedCache, IConfiguration configuration, object result, string key)
        {
            var expireMinute = !string.IsNullOrEmpty(configuration["ConfigApp:CachingExpireMinute"]) ? int.Parse(configuration["ConfigApp:CachingExpireMinute"]) : 1;
            var data = JsonConvert.SerializeObject(result);
            var expireTime = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(expireMinute) };
            distributedCache.SetStringAsync(key, data, expireTime);
        }

        public static void SetCacheData(IDistributedCache distributedCache, int expireMinute, object result, string key)
        {
            var data = JsonConvert.SerializeObject(result);
            var expireTime = new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(expireMinute) };
            distributedCache.SetStringAsync(key, data, expireTime);
        }

        public static void RemoveCache(IDistributedCache distributedCache, string key)
        {
            distributedCache.Remove(key);
        }
    }
}

using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace WebCore.Utils
{
    public static class CachedUtils
    {
        public static List<T> GetCacheOf<T>(string newHash)
        {
            return GetCache<List<T>>(typeof(T).Name, newHash);
        }

        public static void SetCacheOf<T>(List<T> cachedList, string newHash)
        {
            SetCache(typeof(T).Name, cachedList, newHash);
        }

        public static string CalcHash<T>(List<T> cachedList)
        {
            using (var memStream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(typeof(List<T>));
                serializer.WriteObject(memStream, cachedList);
                var buffer = memStream.ToArray();
                return CommonUtils.MD5Standard(buffer);
            }
        }

        private static T GetCache<T>(string cachedName, string newHash)
        {
            try
            {
                var fileName = string.Format("Cache\\{0}.{1}.cache", cachedName, newHash);
                using(var stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var serializer = new DataContractSerializer(typeof(T));
                    return (T)serializer.ReadObject(stream);
                }
            }
            catch
            {
                return default(T);                
            }
        }

        private static void SetCache<T>(string cachedName, T cacheObject, string newHash)
        {
            try
            {
                Directory.CreateDirectory("Cache");
                var fileName = string.Format("Cache\\{0}.{1}.cache", cachedName, newHash);
                using (var stream = File.Open(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var serializer = new DataContractSerializer(typeof(T));
                    serializer.WriteObject(stream, cacheObject);
                }
            }
            catch
            {
            }
        }
    }
}

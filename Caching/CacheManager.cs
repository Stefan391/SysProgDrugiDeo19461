using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysProgDrugiDeo19461.Caching
{
    public static class CacheManger
    {
        private static ConcurrentDictionary<string, string> _cache = new();
        public static string GetItem(string key)
        {
            _cache.TryGetValue(key, out var value);

            return value ?? "";
        }

        public static void SetItem(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
                return;

            _cache.TryAdd(key, value);
        }
    }
}

using System.Runtime.Caching;

namespace DemoLLD.Service.Services.Cache;

public class InMemoryCache
{
    private MemoryCache _memoryCache;

    public InMemoryCache()
    {
        _memoryCache = new MemoryCache("AppCache");
    }

    public void Set(string key, object value, TimeSpan slidingExpiration)
    {
        var policy = new CacheItemPolicy
        {
            
            SlidingExpiration = slidingExpiration
        };
        _memoryCache.Set(key, value, policy);
    }

    public object Get(string key)
    {
        var cacheItem = _memoryCache.Get(key);
        if (cacheItem == null)
        {
            throw new NullReferenceException("Cache missed or expired");
        }
        return cacheItem;
    }

    public void Remove(string key)
    {
        _memoryCache.Remove(key);
    }
}

public record class Repos(string name);

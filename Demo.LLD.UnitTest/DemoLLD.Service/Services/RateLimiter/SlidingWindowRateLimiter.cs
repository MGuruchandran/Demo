using DemoLLD.Service.Models;
using System.Collections.Concurrent;
namespace DemoLLD.Service.Services.RateLimiter;

public class SlidingWindowRateLimiter
{
    private readonly int _capacity;
    private readonly TimeSpan _timeSpan;
    private ConcurrentDictionary<string, UserInfo> _userRequest;
    private readonly object lockObj = new();

    public SlidingWindowRateLimiter(int capacity, TimeSpan timeSpan)
    {
        _capacity = capacity;
        _timeSpan = timeSpan;
        _userRequest = new();
    }

    public bool TryRequest(string userId, string request)
    {
        var user = _userRequest.GetOrAdd(userId, new UserInfo());
        lock (user.Lock)
        {
            DateTimeOffset now = DateTimeOffset.UtcNow;
            DateTimeOffset windowStart = now - _timeSpan;

            while (user.TimeStamps.Count > 0 && user.TimeStamps.Peek() < windowStart)
            {
                user.TimeStamps.Dequeue();
            }

            if (user.TimeStamps.Count < _capacity)
            {
                user.TimeStamps.Enqueue(DateTimeOffset.Now);
                return true;
            }

            return false;
        }
    }
}

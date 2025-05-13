using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoLLD.Service.Services.RateLimiter;

public class TokenBucket
{
    private readonly int capacity;
    private readonly int tokenrefillrate;
    private int currentTokens;
    private DateTimeOffset lastRefillTime;
    private readonly object lockObj = new object();

    public TokenBucket(int capacity, int refillrate)
    {
        this.capacity = capacity;
        this.tokenrefillrate = refillrate;
        this.currentTokens = capacity;
        this.lastRefillTime = DateTimeOffset.UtcNow;
    }

    public bool TryAddRequest()
    {
        lock (lockObj)
        {
            RefillToken();

            if (currentTokens > 0)
            {
                currentTokens--;
                return true;
            }

            return false;
        }

    }

    public int GetCurrentTokenCount()
    {
        lock (lockObj)
        {
            RefillToken();
            return currentTokens;
        }
    }


    private void RefillToken()
    {
        lock (lockObj)
        {
            var now = DateTimeOffset.UtcNow;
            var timeElapsed = (now - lastRefillTime).TotalSeconds;

            int tokensAdded = (int)(timeElapsed * tokenrefillrate);

            currentTokens = Math.Min(currentTokens + tokensAdded, capacity);
            lastRefillTime = now;
        }
    }
}

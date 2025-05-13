namespace Demo.RateLimiter.Services;

// Tokens are refilled at a fixed rate and reqeust consume tokens
// capacity 5 , refillrate 1
// t0  currentoken 5
// t1 1 request currenttoken = 4, 
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

            if(currentTokens > 0)
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

            currentTokens = Math.Min(currentTokens + tokensAdded,capacity);
            lastRefillTime = now;
        }
    }



}

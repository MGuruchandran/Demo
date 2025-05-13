namespace Demo.RateLimiter.Services;

public class LeakyBucket
{
    private readonly int capacity;   // Max capacity of the bucket
    private readonly int leakRate;   // Number of requests to leak per second
    private int waterLevel;          // Current water level (number of requests in the bucket)
    private DateTimeOffset lastLeakTime;   // Time when the last leak occurred
    private readonly object lockObj = new object(); // Synchronization object to protect shared resources

    public LeakyBucket(int capacity, int leakRate)
    {
        this.capacity = capacity;
        this.leakRate = leakRate;
        this.waterLevel = 0;
        this.lastLeakTime = DateTimeOffset.UtcNow;
    }

    public bool TryAddRequest()
    {
        lock(lockObj)
        {
            LeakRequest();

            if (waterLevel >= capacity)
            {
                return false;
            }

            waterLevel++;
            return true;
        }
    }

    public int GetCurrentWaterLevel()
    {
        lock (lockObj)
        {
            LeakRequest();
            return waterLevel;
        }
    }

    private void LeakRequest()
    {
        var now = DateTimeOffset.UtcNow;
        var timeElapsed = (now - lastLeakTime).TotalSeconds;

        int leakedRequest = (int)(timeElapsed * leakRate);
        waterLevel = Math.Max(waterLevel - leakedRequest, 0);
        lastLeakTime = now;
    }
}

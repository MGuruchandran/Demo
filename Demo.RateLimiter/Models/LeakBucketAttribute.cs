namespace Demo.RateLimiter.Models;

public class LeakBucketAttribute
{
    public int NumberOfQueries { get; set; }
    public int Storage { get; set; }
    public int OutputBucketSize { get; set; }
    public int InputBucketSize { get; set; }
    public int BucketSize { get; set; }
    public int SizeLeft { get; set; }

}

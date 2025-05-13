namespace Demo.RateLimiter.Models;

public class UserInfo
{
    public object Lock { get; } = new object();

    public Queue<DateTimeOffset> TimeStamps { get; } = new Queue<DateTimeOffset>();

}

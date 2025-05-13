using Demo.RateLimiter.Models;

namespace Demo.RateLimiter.Services.LoadBalancing;

public class RoundRobin
{
    private int counter;
    private List<Server> Servers;

    public RoundRobin()
    {
        Servers = new();
        counter = 0;
        
    }

    public void AddServer(string hostName, string ipAddress)
    {
        Servers.Add(new Server() { HostName = hostName,IPAddress=ipAddress});
    }

    public Server GetServer()
    {
        return Servers[counter++ % Servers.Count];
    }
}

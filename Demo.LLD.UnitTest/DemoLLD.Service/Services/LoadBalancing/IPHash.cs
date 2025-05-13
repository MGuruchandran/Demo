using DemoLLD.Service.Models;

namespace DemoLLD.Service.Services.LoadBalancing;

public class IPHash
{
    private readonly List<Server> _servers;

    public IPHash()
    {
        _servers = new List<Server>();
    }

    public void AddServer(string hostName, string ipAddress, bool health)
    {
        _servers.Add(new Server() { HostName = hostName, IPAddress = ipAddress, HealthCheck = health });
    }

    public Server GetNextServer(string clientIp)
    {
        if (_servers.Count == 0) throw new InvalidOperationException("No servers available");

        var hash = clientIp.GetHashCode();
        var serverIdx = Math.Abs(hash)%_servers.Count;

        return _servers[serverIdx];
    }
}

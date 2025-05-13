using DemoLLD.Service.Models;

namespace DemoLLD.Service.Services.LoadBalancing;

public class WeightedRoundRobin
{
    private readonly IList<WeightedServer> _servers;
    private int currentIndex;

    public WeightedRoundRobin()
    {
        _servers= new List<WeightedServer>();
        currentIndex = 0;
    }

    public void AddServer(string hostName, string ipAddress, int weight)
    {
        _servers.Add(new WeightedServer() { HostName = hostName, IPAddress = ipAddress, Weight = weight });
    }

    public WeightedServer GetNextServer()
    {
        if (_servers.Count == 0) throw new InvalidOperationException("No servers available");
       
        int totalWeight = _servers.Sum(s=>s.Weight);
        int rndValue = new Random().Next(0,totalWeight);

        int accumulatedWeight = 0;

        foreach (var server in _servers)
        {
            accumulatedWeight += server.Weight;
            if (rndValue < accumulatedWeight)
            {
                return server;
            }
        }

        return _servers.Last();
        
    }

}

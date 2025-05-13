using DemoLLD.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoLLD.Service.Services.LoadBalancing;

public class ConsistentHashing
{
    private readonly List<Server> _servers;
    private SortedDictionary<int, Server> hashRing;

    public ConsistentHashing()
    {
        _servers = new();
        hashRing = new SortedDictionary<int, Server>();
    }

    public void AddServer(Server server)
    {
        var hash = server.IPAddress.GetHashCode();
        _servers.Add(server);
        hashRing[hash] = server;
    }

    public void RemoveServer(Server server)
    {
        var hash = server.IPAddress.GetHashCode();
        _servers.Remove(server);
        hashRing.Remove(hash);
    }

    public Server GetNextServer(string key)
    {
        if (hashRing.Count == 0) throw new InvalidOperationException("No Server available");

        var hash = key.GetHashCode();
        var serverHash = hashRing.Keys.FirstOrDefault(h=> h>= hash);
        if (serverHash == 0) {
            serverHash = hashRing.Keys.First();
        }

        return hashRing[serverHash];
    }
}

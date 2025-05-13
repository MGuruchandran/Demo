using DemoLLD.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoLLD.Service.Services.LoadBalancing;

public class LeastConnection
{
    private readonly IList<LeastConnectionServer> _servers;

    public LeastConnection()
    {
        _servers = new List<LeastConnectionServer>();
    }

    public void AddServer(string hostName, string ipAddress, int activeConnection)
    {
        _servers.Add(new LeastConnectionServer() { HostName = hostName, IPAddress = ipAddress, ActiveConnection = activeConnection });
    }

    public LeastConnectionServer GetNextServer()
    {
        if (_servers.Count == 0) throw new InvalidOperationException("No servers available");

        var server = _servers.OrderBy(m => m.ActiveConnection).Where(m=>m.ActiveConnection > m.CurrentConnection);
        if (server == null || server.Count() == 0 ) throw new InvalidOperationException("No servers available");

        var leastServer = server.FirstOrDefault();
        leastServer.CurrentConnection++;

        return leastServer;

    }
}

using DemoLLD.Service.Models;

namespace DemoLLD.Service.Services.LoadBalancing;
public class RoundRobin
{
    private int counter;
    private readonly List<Server> Servers = new List<Server>();

    

    public RoundRobin()
    {
        //Servers = new();
        counter = 0;

    }

    public void AddServer(string hostName, string ipAddress, bool health)
    {
        Servers.Add(new Server() { HostName = hostName, IPAddress = ipAddress,HealthCheck=health });
    }

    public Server GetServer()
    {
        if (Servers.Count == 0) throw new InvalidOperationException("No servers available.");
        int strIdx = counter;
        do
        {
            var server = Servers[counter++ % Servers.Count];
            if (server.HealthCheck) {
                return server;
            }
        } while (strIdx != counter);
        

        throw new InvalidOperationException("No healthy servers available");
    }
}

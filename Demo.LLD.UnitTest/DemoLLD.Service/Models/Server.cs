namespace DemoLLD.Service.Models;

public class Server
{
    public string HostName { get; set; } = string.Empty;
    public string IPAddress { get; set; } = string.Empty;

    public bool HealthCheck { get; set; }
}

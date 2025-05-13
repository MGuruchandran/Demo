using DemoLLD.Service.Models;
using DemoLLD.Service.Services.LoadBalancing;
using FluentAssertions;
using Moq;
using System.Net;

namespace Demo.DDL.Services.Test;

public class RoundRobinTest
{
    private readonly Mock<RoundRobin> _roundRobin;
    private readonly RoundRobin _roundRobincls;
    public RoundRobinTest()
    {
        _roundRobin = new Mock<RoundRobin>();
        _roundRobincls = new RoundRobin();

    }

    [Fact]
    public void returns_server_roundrobin()
    {
        //Arrange
        //_roundRobincls = new RoundRobin();

        //Act
        List<Server> servers = new List<Server>()
        {
            new Server(){ HostName = "HostName1",IPAddress = "IPAddress1"},
            new Server(){ HostName = "HostName2",IPAddress = "IPAddress2"},
            new Server(){ HostName = "HostName3",IPAddress = "IPAddress3"},
            new Server(){ HostName = "HostName4",IPAddress = "IPAddress4"},
            new Server(){ HostName = "HostName5",IPAddress = "IPAddress5"},
        };
        _roundRobincls.AddServer("HostName1", "IPAddress1",true);
        _roundRobincls.AddServer("HostName2", "IPAddress2", true);
        _roundRobincls.AddServer("HostName3", "IPAddress3", true);
        _roundRobincls.AddServer("HostName4", "IPAddress4", true);
        _roundRobincls.AddServer("HostName5", "IPAddress5", true);

        for (int i = 0; i < 10; i++) {
            //Assert
            Assert.Equal(servers[i%servers.Count].HostName,_roundRobincls.GetServer().HostName);
        }

    }

    [Fact]
    public void GetServer_ShouldHandleEmptyService()
    {
        var action1 = () => _roundRobincls.GetServer();
        action1.Should().Throw<InvalidOperationException>().WithMessage("No servers available.");
        Assert.Throws<InvalidOperationException>(()=>_roundRobincls.GetServer());
    }
}

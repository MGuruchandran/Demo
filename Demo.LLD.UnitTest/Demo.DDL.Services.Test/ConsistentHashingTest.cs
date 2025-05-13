

using DemoLLD.Service.Models;
using DemoLLD.Service.Services.LoadBalancing;
using FluentAssertions;

namespace Demo.DDL.Services.Test;

public class ConsistentHashingTest
{
    private ConsistentHashing _consistentHashing;

    public ConsistentHashingTest()
    {
        _consistentHashing = new ConsistentHashing();
    }

    [Fact]
    public void GetNextServer_ShouldReturnValidServer()
    {
        //Arrange
        _consistentHashing.AddServer(new Server() {HealthCheck=true,HostName="HostName1",IPAddress = "IPAddress1" });
        _consistentHashing.AddServer(new Server() { HealthCheck = true, HostName = "HostName2", IPAddress = "IPAddress2" });
        _consistentHashing.AddServer(new Server() { HealthCheck = true, HostName = "HostName3", IPAddress = "IPAddress3" });
        //Act
        var server =  _consistentHashing.GetNextServer("userId2122");

        //Assert
        server.Should().NotBeNull();
        server.HostName.Should().NotBeNull();
    }
}

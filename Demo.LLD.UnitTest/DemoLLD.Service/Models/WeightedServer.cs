using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoLLD.Service.Models;

public class WeightedServer
{
    public string HostName { get; set; } = string.Empty;
    public string IPAddress { get; set; } = string.Empty;
    public int Weight { get; set; }
}

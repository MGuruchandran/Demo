using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoLLD.Service.Models;

public class UserInfo
{
    public object Lock { get; } = new object();

    public Queue<DateTimeOffset> TimeStamps { get; } = new Queue<DateTimeOffset>();
}

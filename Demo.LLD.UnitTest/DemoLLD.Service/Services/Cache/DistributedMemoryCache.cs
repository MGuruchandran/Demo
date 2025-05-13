using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoLLD.Service.Services.Cache;

public class DistributedMemoryCache
{
    private ConnectionMultiplexer _connectionMultiplexer;
    private IDatabase _database;

    public DistributedMemoryCache(string connectionString)
    {
        _connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
        _database = _connectionMultiplexer.GetDatabase();
    }

    //public void Set<T>(string key, T value)
    //{
    //    RedisKey redisKey = key;
    //    RedisValue redisValue = value;
    //    KeyValuePair<RedisKey, RedisValue> keyValuePair = new KeyValuePair<RedisKey, RedisValue>();

    //    _database.StringSet();
    //}
}

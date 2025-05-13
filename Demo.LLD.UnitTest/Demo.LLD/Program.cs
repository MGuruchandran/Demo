// See https://aka.ms/new-console-template for more information
using DemoLLD.Service.Models;
using DemoLLD.Service.Services.Cache;
using DemoLLD.Service.Services.DesignPattern;
using DemoLLD.Service.Services.Handler;
using DemoLLD.Service.Services.LoadBalancing;
using DemoLLD.Service.Services.Queue;
using System.Net.Http.Headers;
using System.Text.Json;

Console.WriteLine("Hello, World!");


var loadBalancing = new RoundRobin();
loadBalancing.AddServer("HostName1","IPAddress1",false);
loadBalancing.AddServer("HostName2", "IPAddress2",true);
loadBalancing.AddServer("HostName3", "IPAddress3",true);
loadBalancing.AddServer("HostName4", "IPAddress4",false);
loadBalancing.AddServer("HostName5", "IPAddress5",true);

for (int i = 1; i <= 10; i++) {
    Console.WriteLine(loadBalancing.GetServer().HostName);
}

Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------");

var leastConnection = new LeastConnection();
leastConnection.AddServer("HostName1", "IPAddress1", 0);
leastConnection.AddServer("HostName2", "IPAddress2", 1);
leastConnection.AddServer("HostName3", "IPAddress3", 2);
leastConnection.AddServer("HostName4", "IPAddress4", 1);
leastConnection.AddServer("HostName5", "IPAddress5", 0);

for (int i = 1; i <= 4; i++)
{
    Console.WriteLine(leastConnection.GetNextServer().HostName);
}


Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------");

var weightedRoundRobin = new WeightedRoundRobin();
weightedRoundRobin.AddServer("HostName1", "IPAddress1", 0);
weightedRoundRobin.AddServer("HostName2", "IPAddress2", 10);
weightedRoundRobin.AddServer("HostName3", "IPAddress3", 2);
weightedRoundRobin.AddServer("HostName4", "IPAddress4", 1);
weightedRoundRobin.AddServer("HostName5", "IPAddress5", 1);

for (int i = 1; i <= 24; i++)
{
    Console.WriteLine(weightedRoundRobin.GetNextServer().HostName);
}


Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------");

var consistentHashing = new ConsistentHashing();
consistentHashing.AddServer(new Server(){ HostName="HostName1", IPAddress="IPAddress1", HealthCheck = true });
consistentHashing.AddServer(new Server() { HostName = "HostName2", IPAddress = "IPAddress2", HealthCheck = true });
consistentHashing.AddServer(new Server() { HostName = "HostName3", IPAddress = "IPAddress3", HealthCheck = true });
consistentHashing.AddServer(new Server() { HostName = "HostName4", IPAddress = "IPAddress4", HealthCheck = true });
consistentHashing.AddServer(new Server() { HostName = "HostName5", IPAddress = "IPAddress5", HealthCheck = true });

for (int i = 1; i <= 24; i++)
{
    Console.WriteLine(consistentHashing.GetNextServer(i.ToString()).HostName);
}


Console.WriteLine("------------------------------------------------------------------------------------------------------------------------------------");

var cache = new InMemoryCache();
cache.Set("user123", new { Item1 = "Value",Item2="Value2"},TimeSpan.FromSeconds(4));
var data = cache.Get("user123");
var json  = JsonSerializer.Serialize<object>(data);
Console.WriteLine(json);
Console.WriteLine(data ?? "notn null");
//System.Threading.Thread.Sleep(9000);  // Wait for 5 seconds
//data = cache.Get("user123");

//Console.WriteLine(data ?? "No data");



//var topic = new Topic("myTopic",2);
//var producer1 = new Producer(topic);
//var producer2 = new Producer(topic);

//var consumer1 = new Consumer(topic, 0);
//var consumer2 = new Consumer(topic, 1);
//var consumer3 = new Consumer(topic, 2);

//// Start consumer threads
//var consumerThread1 = new Thread(() => consumer1.Consume());
//var consumerThread2 = new Thread(() => consumer2.Consume());
//var consumerThread3 = new Thread(() => consumer3.Consume());

//consumerThread1.Start();
//consumerThread2.Start();
//consumerThread3.Start();

//// Simulate producers sending messages
//producer1.Produce("Message 1");
//producer2.Produce("Message 2");
//producer1.Produce("Message 3");
//producer2.Produce("Message 4");
//producer1.Produce("Message 5");

//// Give consumers some time to process messages
//Thread.Sleep(5000);

//// Terminate consumers (not a good practice in real code, just for this example)
//consumerThread1.Suspend();
//consumerThread2.Abort();
//consumerThread3.Abort();


var subject = new Subject();
var subject2 = new Subject();

// Create observers
var observer1 = new Observer("Observer 1");
var observer2 = new Observer("Observer 2");

// Subscribe observers to the subject
subject.Subscribe(observer1);
subject.Subscribe(observer2);

// Notify observers with new values
subject.Notify(new MessageInfo() { Message = "First Message",TimeStamp = DateTimeOffset.UtcNow});
subject.Notify(new MessageInfo() { Message = "Second Message", TimeStamp = DateTimeOffset.UtcNow });
subject.Subscribe(new Observer("Observer 3"));
subject2.Subscribe(new Observer("Observer 3"));
// Unsubscribe observer1
var unsubscriber = subject.Subscribe(observer1);
unsubscriber.Dispose();

// Notify observers after unsubscription
subject.Notify(new MessageInfo() { Message = "Third Message", TimeStamp = DateTimeOffset.UtcNow });
subject2.Notify(new MessageInfo() { Message = "First Message from subject 2", TimeStamp = DateTimeOffset.UtcNow });


using var client = new HttpClient(new MessageHandlerStepOne());

//client.DefaultRequestHeaders.Clear();
//client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
//client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

var response = await client.GetAsync("https://api.github.com/orgs/dotnet/repos");
Console.WriteLine(response.IsSuccessStatusCode);
await using Stream stream = await response.Content.ReadAsStreamAsync();
//await using Stream stream = await client.GetStreamAsync("https://api.github.com/orgs/dotnet/repos");
var repos = await JsonSerializer.DeserializeAsync<List<Repos>>(stream);
foreach (var item in repos)
{
    Console.WriteLine(item.name);
}


ValidSudoku validSudoku = new();
string[][] board = [["5", "3", ".", ".", "7", ".", ".", ".", "."], ["6", ".", ".", "1", "9", "5", ".", ".", "."], [".", "9", "8", ".", ".", ".", ".", "6", "."], ["8", ".", ".", ".", "6", ".", ".", ".", "3"], ["4", ".", ".", "8", ".", "3", ".", ".", "1"], ["7", ".", ".", ".", "2", ".", ".", ".", "6"], [".", "6", ".", ".", ".", ".", "2", "8", "."], [".", ".", ".", "4", "1", "9", ".", ".", "5"], [".", ".", ".", ".", "8", ".", ".", "7", "9"]];
Console.WriteLine(validSudoku.IsValidSudoku([["5","3",".",".","7",".",".",".","."],["6",".",".","1","9","5",".",".","."],[".","9","8",".",".",".",".","6","."],["8",".",".",".","6",".",".",".","3"],["4",".",".","8",".","3",".",".","1"],["7",".",".",".","2",".",".",".","6"],[".","6",".",".",".",".","2","8","."],[".",".",".","4","1","9",".",".","5"],[".",".",".",".","8",".",".","7","9"]]));
Console.WriteLine(validSudoku.SolveSudoku(board));







using System.Collections.Concurrent;

namespace DemoLLD.Service.Services.Queue;

public class DistributedQueue
{
    private readonly ConcurrentQueue<Message> _queue = new ConcurrentQueue<Message>();
    private readonly AutoResetEvent _messageAvailableEvent = new AutoResetEvent(false);

    public void EnqueueMessage(Message message)
    {
        _queue.Enqueue(message);
        _messageAvailableEvent.Set();
    }

    public Message? DequeueMessage()
    {
        _messageAvailableEvent.WaitOne();
        if (_queue.TryDequeue(out Message? message))
        {
            return message;
        }

        return null;
    }

    public bool HasMessage()
    {
        return !_queue.IsEmpty;
    }
}

public class Message
{
    public string Content { get; set; }
    public DateTimeOffset TimeStamp { get; set; }

    public Message(string content)
    {
        Content = content;
        TimeStamp = DateTimeOffset.UtcNow;
    }
}

public class Topic
{
    public string Name { get; set; }
    public List<DistributedQueue> Partition { get; set; }

    public Topic(string name, int numPartition)
    {
        Name = name;
        Partition = new List<DistributedQueue>();
        for(int i = 0; i < numPartition; i++) 
        {
            Partition.Add(new DistributedQueue());
        }
    }

    public DistributedQueue GetPartition(int partitonId)
    {
        return Partition[partitonId%Partition.Count];
    }
}

public class Producer
{
    private readonly Topic _topic;
    public Producer(Topic topic)
    {
        _topic = topic;
    }

    public void Produce(string messageContent) 
    {
        var message = new Message(messageContent);
        int partId = GetPartitionMessage(message);
        var partition = _topic.GetPartition(partId);
        partition.EnqueueMessage(message);
    }

    private int GetPartitionMessage(Message message)
    {
        return message.Content.GetHashCode();
    }
}

public class Consumer
{
    private readonly Topic _topic;
    private readonly int _partitionId;

    public Consumer(Topic topic, int partitionId)
    {
        _topic = topic;
        _partitionId = partitionId;
    }

    public void Consume()
    {
        var partition = _topic.GetPartition(_partitionId);
        while (true)
        {
            var message = partition.DequeueMessage();
            if (message != null) 
            {
                Console.WriteLine($"Consumer: Consumed message from topic '{_topic.Name}', partition {_partitionId}: {message.Content}");
            }
        }

    }
}



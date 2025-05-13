public class LRUCache
{
    private int _size;
    private int _capacity;
    Dictionary<int, DoubleLinkedNode> _cache;
    DoubleLinkedNode _head;
    DoubleLinkedNode _tail;

    public LRUCache(int capacity)
    {
        _size = 0;
        _capacity = capacity;
        _cache = new Dictionary<int, DoubleLinkedNode>();
        _head = new DoubleLinkedNode();
        _tail = new DoubleLinkedNode();
        _head.Next = _tail;
        _tail.Prev = _head;
    }




    public void AddNode(DoubleLinkedNode node)
    {
        node.Prev = _head;
        node.Next = _head.Next;
        _head.Next.Prev = node;
        _head.Next = node;
    }

    public void RemoveNode(DoubleLinkedNode node)
    {
        var prev = node.Prev;
        var next = node.Next;
        prev.Next = next;
        next.Prev = prev;
    }

    public void MoveToHead(DoubleLinkedNode node)
    {
        RemoveNode(node);
        AddNode(node);
    }

    public DoubleLinkedNode Pop()
    {
        DoubleLinkedNode node = _tail.Prev;
        RemoveNode(node);
        return node;
    }

    public int Get(int key)
    {
        if (!_cache.ContainsKey(key)) return -1;
        var node = _cache[key];
        MoveToHead(node);
        return node.Value;
    }
    public void Put(int key, int value)
    {
        if (!_cache.ContainsKey(key))
        {
            var node = new DoubleLinkedNode();
            node.Key = key;
            node.Value = value;
            _cache.Add(key, node);
            AddNode(node);
            ++_size;
            if (_size > _capacity)
            {
                var tail = Pop();
                _cache.Remove(tail.Key);
                _size--;

            }
        }
        else
        {
            var node = _cache[key];
            node.Value = value;
            MoveToHead(node);
        }
    }

}

public class DoubleLinkedNode
{
    public int Key { get; set; }
    public int Value { get; set; }
    public DoubleLinkedNode Prev { get; set; }
    public DoubleLinkedNode Next { get; set; }

    
}
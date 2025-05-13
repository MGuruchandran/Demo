using System.Runtime.CompilerServices;
using System.Threading.Channels;

public class TrieNode
{
    public bool IsWord { get; set; }
    public Dictionary<char, TrieNode> Children { get; } = new();
}

public class Trie
{
    private readonly TrieNode _root = new();
    public void Add(string word)
    {
        var node = _root;
        foreach (var c in word)
        {
            if (!node.Children.ContainsKey(c)) node.Children[c] = new TrieNode();
            node = node.Children[c];
        }
        node.IsWord = true;
    }
    public bool Search(string word)
    {
        var node = _root;
        foreach (var c in word)
        {
            if (!node.Children.ContainsKey(c)) return false;
            node = node.Children[c];
        }
        return node.IsWord;
    }
}
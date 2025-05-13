using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoLLD.Service.Services.DesignPattern;

public class Subject : IObservable<MessageInfo>
{
    private readonly HashSet<IObserver<MessageInfo>> _observers = new();
    private readonly HashSet<MessageInfo> _info = new();
    public IDisposable Subscribe(IObserver<MessageInfo> observer)
    {
        if (_observers.Add(observer))
        {
            foreach (MessageInfo item in _info)
            {
                observer.OnNext(item);
            }
        }
        return new Unsubscribe(_observers, observer);
    }

    public void Notify(MessageInfo messageInfo)
    {
        foreach (var observer in _observers)
        {
            observer.OnNext(messageInfo);
        }
    }
}

public class Unsubscribe : IDisposable
{
    private readonly ISet<IObserver<MessageInfo>> _observers;
    private readonly IObserver<MessageInfo> _info;

    internal Unsubscribe(ISet<IObserver<MessageInfo>> observers, IObserver<MessageInfo> observer) => (_observers, _info) = (observers,observer);
    public void Dispose() => _observers.Remove(_info);
}

public readonly record struct MessageInfo(string Message, DateTimeOffset TimeStamp);

public class Observer : IObserver<MessageInfo>
{
    private readonly string _name;
    public Observer(string name)
    {
        _name = name;
    }
    public void OnCompleted()
    {
        Console.WriteLine($"{_name} has finished receiving values.");
    }

    public void OnError(Exception error)
    {
        Console.WriteLine($"{_name} received message: {error.Message}");
    }

    public void OnNext(MessageInfo value)
    {
        Console.WriteLine($"{_name} received message: {value.Message}");
    }
}

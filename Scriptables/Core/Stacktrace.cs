using ScriptableArchitecture.Core;
using System;
using System.Collections.Generic;
using System.Linq;

public class Stacktrace
{
    private Queue<string> _messages = new Queue<string>();

    private VariableType _stackType;
    private int _maxCapacity;

    public Stacktrace(VariableType stackType, int maxCapacity = 100)
    {
        _stackType = stackType;
        _maxCapacity = Math.Max(1, maxCapacity);
    }

    public void Add(string message)
    {
        if (_messages.Count >= _maxCapacity)
            _messages.Dequeue();

        _messages.Enqueue(DateTime.Now.ToString("HH:mm:ss") + ": " + message);
    }

    public void Clear() => _messages.Clear();

    public List<string> GetMessages() => _messages.ToList();

    public VariableType GetStackType() => _stackType;
}
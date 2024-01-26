using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScriptableArchitecture.Core
{
    public class GameEventBase : ScriptableObject, IGameEvent
    {
        private List<IListenerEmpty> _listeners = new List<IListenerEmpty>();

        public void Raise()
        {
            Log("Raised", null);
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].OnEventRaised();
                Log($"Raised", _listeners[i]);
            }
        }

        public void RegisterListener(IListenerEmpty listener)
        {
            if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
                Log($"Registered listener", listener);
            }
            else
            {
                Log($"Tried registering an existing listener", listener);
            }
        }

        public void UnregisterListener(IListenerEmpty listener)
        {
            if (_listeners.Contains(listener))
            {
                _listeners.Remove(listener);
                Log($"Unregistered listener", listener);
            }
            else
            {
                Log($"Tried removing a non existing listener", listener);
            }   
        }

        public void RemoveAllListeners()
        {
            _listeners.Clear();
            Log("Cleared all listeners", null);
        }

        public List<IListener> GetListeners() => _listeners.Cast<IListener>().ToList();


        //Stack trace

        private void OnEnable()
        {
#if UNITY_EDITOR
            _stacktrace.Clear();
#endif
        }

        private void Log(string message, IListener listener)
        {
#if UNITY_EDITOR
            _stacktrace.Add($"{message}" + (listener != null ? $" ({listener.GetName()})" : ""));
#endif
        }

        public Stacktrace[] GetStackTraces() => new Stacktrace[] { _stacktrace };

#if UNITY_EDITOR
        private Stacktrace _stacktrace = new Stacktrace(VariableType.Event, 100);
#endif
    }

    public interface IGameEvent
    {
        public List<IListener> GetListeners();

        public Stacktrace[] GetStackTraces();
    }

    public interface IGameEvent<T> : IGameEvent
    {
        public void RegisterListener(IListener<T> listener);
        public void UnregisterListener(IListener<T> listener);
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ScriptableArchitecture.Core
{
    public abstract class Variable<T> : Variable, IGameEvent<T>
    {
        //Variable

        public InitializeType InitializeTypeVariable;

        [SerializeField] private T _value;
        [SerializeField] private T _startValue;

        public T Value
        {
            get 
            {
                if (InitializeTypeVariable == InitializeType.ReadOnly)
                    return _startValue;

                return _value;
            }
            set 
            {
                if (InitializeTypeVariable == InitializeType.ReadOnly)
                {
                    Debug.LogWarning($"Cannot set: {name} (Readonly)");
                    return;
                }

                _value = value;
                Log(_stacktraceVariable, $"Set value to: {Value}");
            }
        }


        //Event
        private List<IListener<T>> _listeners;


        //RuntimeSet
        public InitializeType InitializeTypeRuntimeSet;

        [SerializeField] private List<T> _runtimeSet;
        [SerializeField] private List<T> _startRuntimeSet;

        public List<T> RuntimeSet
        {
            get
            {
                if (InitializeTypeRuntimeSet == InitializeType.ReadOnly)
                    return _startRuntimeSet;

                return _runtimeSet;
            }
        }


        //OnEnable is called when the first scene is loaded, regardles if the scene has a reference to it (build game)
        private void OnEnable()
        {
            if (VariableType == VariableType.Variable || VariableType == VariableType.VariableEvent)
            {
                //This is only for the editor as the build game does not save Scriptable objects across sessions
                if (InitializeTypeVariable == InitializeType.ResetOnGameStart)
                {
                    _value = _startValue;
                    Log(_stacktraceVariable, $"Set value to startvalue: {Value}");
                }
            }

            if (VariableType == VariableType.Event || VariableType == VariableType.VariableEvent)
            {
                //Only initilize lists when needed to avoid overhead
                _listeners = new List<IListener<T>>();
            }
            
            if (VariableType == VariableType.RuntimeSet)
            {
                if (InitializeTypeRuntimeSet == InitializeType.ResetOnGameStart)
                {
                    _runtimeSet = new List<T>(_startRuntimeSet);
                    Log(_stacktraceRuntimeSet, $"Set runtimeSet to startvalue");
                }

                if (InitializeTypeRuntimeSet == InitializeType.Normal)
                    _runtimeSet = new List<T>();
            }


#if UNITY_EDITOR
            _stacktraceEvent.Clear();
#endif
        }


        //Variable

        public void Set(T value)
        {
            Value = value;
            Log(_stacktraceVariable, $"Set value to {Value}");
        }


        //Event

        public void Raise(T value)
        {
            Value = value;
            Log(_stacktraceEvent, $"Raised and set value to {Value}");

            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].OnEventRaised(value);
                LogListener(_stacktraceEvent, $"Raised value: {value}", _listeners[i]);
            }
        }

        public void Raise()
        {
            Raise(Value);
        }

        public void RegisterListener(IListener<T> listener)
        {
            if (!_listeners.Contains(listener))
            {
                _listeners.Add(listener);
                LogListener(_stacktraceEvent, $"Registered listener", listener);
            }
            else
            {
                LogListener(_stacktraceEvent, $"Tried registering an existing listener", listener);
            }
        }

        public void UnregisterListener(IListener<T> listener)
        {
            if (_listeners.Contains(listener))
            {
                _listeners.Remove(listener);
                LogListener(_stacktraceEvent, $"Unregistered listener", listener);
            }
            else
            {
                LogListener(_stacktraceEvent, $"Tried removing a non existing listener", listener);
            }
        }

        public void RemoveAllListeners()
        {
            _listeners.Clear();
            Log(_stacktraceEvent, "Cleared all listeners");
        }

        public List<IListener> GetListeners() => _listeners.Cast<IListener>().ToList();


        //RuntimeSet

        public void Add(T value)
        {
            if (InitializeTypeRuntimeSet == InitializeType.ReadOnly)
            {
                Debug.LogWarning($"Cannot add {value} to: {name} (Readonly)");
                return;
            }

            _runtimeSet.Add(value);
            Log(_stacktraceRuntimeSet, $"Added value to runtimeSet: {value}");
        }

        public void Remove(T value)
        {
            if (InitializeTypeRuntimeSet == InitializeType.ReadOnly)
            {
                Debug.LogWarning($"Cannot remove {value} from: {name} (Readonly)");
                return;
            }

            _runtimeSet.Remove(value);
            Log(_stacktraceRuntimeSet, $"Removed value from runtimeSet: {value}");
        }

        public void ClearRuntimeSet()
        {
            if (InitializeTypeRuntimeSet == InitializeType.ReadOnly)
            {
                Debug.LogWarning($"Cannot clear {name} (Readonly)");
                return;
            }

            _runtimeSet.Clear();
            Log(_stacktraceRuntimeSet, $"Cleared runtimeSet");
        }


        //Debugging

        private void Log(Stacktrace stacktrace, string message)
        {
#if UNITY_EDITOR
            stacktrace.Add(message);
#endif
        }

        private void LogListener(Stacktrace stacktrace, string message, IListener listener)
        {
#if UNITY_EDITOR
            stacktrace.Add($"{message}" + (listener != null ? $" ({listener.GetName()})" : ""));
#endif
        }

        public Stacktrace[] GetStackTraces() => new Stacktrace[] { _stacktraceVariable, _stacktraceEvent, _stacktraceRuntimeSet };

        private Stacktrace _stacktraceVariable = new Stacktrace(VariableType.Variable, 14);
        private Stacktrace _stacktraceEvent = new Stacktrace(VariableType.Event, 100);
        private Stacktrace _stacktraceRuntimeSet = new Stacktrace(VariableType.RuntimeSet, 100);

        public T DebugValue;
    }

    public abstract class Variable : ScriptableObject 
    {
        public VariableType VariableType;
    }
}
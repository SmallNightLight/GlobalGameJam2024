using UnityEngine;
using UnityEngine.Events;

namespace ScriptableArchitecture.Core
{
    public abstract class GameEventListenerBase : MonoBehaviour, IListenerEmpty
    {
        [SerializeField] private GameEventBase _event;
        [SerializeField] private UnityEvent _response;
        
        public void OnEventRaised()
        {
            _response.Invoke();
        }

        private void OnEnable()
        {
            if (_event == null)
            {
                Debug.LogWarning($"No Event selected on {name}");
                return;
            }
                
            _event.RegisterListener(this);
        }

        private void OnDisable()
        {
            if (_event == null) return;

            _event.UnregisterListener(this);
        }

        public Object GetListenerObject() => this;

        public IGameEvent GetGameEvent() => _event;

        public string GetName() => name;
    }

    public abstract class GameEventListenerBase<T> : MonoBehaviour, IListener<T>
    {
        [SerializeField] private UnityEvent<T> _response;

        public void OnEventRaised(T value)
        {
            _response.Invoke(value);
        }

        private void OnEnable()
        {
            IGameEvent<T> genericEvent = GetGameEventT();

            if (genericEvent == null)
            {
                Debug.LogWarning($"No Event selected on {name}");
                return;
            }

            genericEvent.RegisterListener(this);
        }

        private void OnDisable()
        {
            IGameEvent<T> genericEvent = GetGameEventT();

            if (genericEvent == null)
                return;

            genericEvent.UnregisterListener(this);
        }

        public Object GetListenerObject() => this;

        public abstract IGameEvent<T> GetGameEventT();

        public IGameEvent GetGameEvent() => GetGameEventT();

        public string GetName() => name;
    }

    public interface IListener 
    {
        public Object GetListenerObject();

        public IGameEvent GetGameEvent();

        public string GetName();
    }

    public interface IListenerEmpty : IListener
    {
        public void OnEventRaised();
    } 

    public interface IListener<T> : IListener
    {
        public void OnEventRaised(T value);

        public IGameEvent<T> GetGameEventT();
    }
}
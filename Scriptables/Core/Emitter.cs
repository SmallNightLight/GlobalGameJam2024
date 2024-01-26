using UnityEngine;
using Object = UnityEngine.Object;

namespace ScriptableArchitecture.Core
{
    public class Emitter : MonoBehaviour
    {
        [SerializeField] private Receiver _receiver;
        [SerializeField] private Object _value;

        [SerializeField] private bool _activateOnStart = true;

        [Tooltip("Higher number for more important emmiters")]
        [SerializeField] private int _priority = 0;

        public int Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = value;
                _receiver?.UpdatePriorityList(this);
            }
        }

        private bool _isAdded;

        public T Value<T>() where T : Object
        {
            if (_value is T)
                return _value as T;

            Debug.LogError($"Could not convert from \"{_value.GetType()} to {typeof(T)} ");
            Debug.Break();
            return null;
        }

        public void SetValue<T>(T value)
        {
            _value = value as Object;
        }

        public void SetValue(Object value)
        {
            _value = value;
        }

        private void OnEnable()
        {
            if (_activateOnStart)
                Register();
        }

        private void OnDisable()
        {
            Unregister();
        }

        public void Register()
        {
            _isAdded = true;
            _receiver.RegisterEmitter(this);
        }

        public void Unregister()
        {
            if (_isAdded)
                _receiver?.UnregisterEmitter(this);

            _isAdded = false;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            _receiver?.UpdatePriorityList(this);
        }
#endif
    }
}
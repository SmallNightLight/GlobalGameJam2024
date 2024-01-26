using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace ScriptableArchitecture.Core
{
    [Serializable]
    public abstract class Reference<T, TVariable> where TVariable : Variable<T>
    {
        [SerializeField] protected bool _isVariable;
        [SerializeField] protected TVariable _variable;
        [SerializeField] protected T _constant;

        //Variable

        public T Value
        {
            get
            {
                if (_isVariable && _variable != null)
                    return _variable.Value;
                else
                    return _constant;
            }
            set
            {
                if (_isVariable && _variable != null)
                    _variable.Value = value;
                else
                {
                    _constant = value;
                    _isVariable = false;
                }
            }
        }


        //Event

        public void Raise(T value)
        {
            if (_isVariable)
                _variable?.Raise(value);
        }

        public void Raise()
        {
            if (_isVariable)
                _variable?.Raise();
        }


        //RuntimeSet

        public List<T> RuntimeSet
        {
            get
            {
                if (_isVariable && _variable != null)
                    return _variable.RuntimeSet;

                return new List<T> { _constant };
            }
        }

        public void Add(T value)
        {
            if (_isVariable)
                _variable?.Add(value);

        }

        public void Remove(T value)
        {
            if (_isVariable)
                _variable?.Remove(value);
        }

        public void Clear()
        {
            if (_isVariable)
                _variable?.ClearRuntimeSet();
        }

        public bool HasItem(T value)
        {
            if (_isVariable && _variable != null)
                return _variable.RuntimeSet.Contains(value);

            return false;
        }
    }
}
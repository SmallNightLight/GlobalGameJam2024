using ScriptableArchitecture.Core;
using UnityEngine;
using ScriptableArchitecture.Data;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class EnemyDeathData : IDataPoint
    {
        public EnemyData EnemyData;
        public Vector2 Position;
    }
}
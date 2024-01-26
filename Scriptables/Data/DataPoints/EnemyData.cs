using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class EnemyData : IDataPoint
    {
        public int Heath;
        public int Damage;
        public float Speed;
        public int Coins;
        public Sprite Sprite;
    }
}
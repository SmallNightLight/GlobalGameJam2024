using ScriptableArchitecture.Core;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class TowerData : IDataPoint
    {
        public string Name;
        public string Description;
        public TowerSingle StartTower;
        public List<TowerSingle> UpgradeTowers;
    }

    [System.Serializable]
    public class TowerSingle
    {
        public Sprite Sprite;
        public int Cost;
        public int Damage;
        public float Range;
        public TowerType TowerType;
    }

    public enum TowerType
    {
        None,
        SingleTarget,
        AOE,
        Debuff
    }
}
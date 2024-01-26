using ScriptableArchitecture.Core;
using System;
using System.Collections.Generic;

namespace ScriptableArchitecture.Data
{
    [System.Serializable]
    public class WaveData : IDataPoint
    {
        [Serializable]
        public class EnemyWave
        {
            public EnemyDataReference Enemy;
            public int Count;
            public float Time;
        }

        public float SpawnSpeed;
        public List<EnemyWave> EnemyData;
    }
}
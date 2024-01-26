using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [AddComponentMenu("GameEvent Listeners/EnemyData Event Listener")]
    public class EnemyDataGameEventListener : GameEventListenerBase<EnemyData>
    {
        [SerializeField] private EnemyDataVariable _event;

        public override IGameEvent<EnemyData> GetGameEventT() => _event;
    }
}
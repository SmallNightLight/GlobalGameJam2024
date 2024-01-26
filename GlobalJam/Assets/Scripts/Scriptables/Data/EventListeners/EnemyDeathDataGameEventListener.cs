using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [AddComponentMenu("GameEvent Listeners/EnemyDeathData Event Listener")]
    public class EnemyDeathDataGameEventListener : GameEventListenerBase<EnemyDeathData>
    {
        [SerializeField] private EnemyDeathDataVariable _event;

        public override IGameEvent<EnemyDeathData> GetGameEventT() => _event;
    }
}
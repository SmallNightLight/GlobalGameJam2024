using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [AddComponentMenu("GameEvent Listeners/TowerData Event Listener")]
    public class TowerDataGameEventListener : GameEventListenerBase<TowerData>
    {
        [SerializeField] private TowerDataVariable _event;

        public override IGameEvent<TowerData> GetGameEventT() => _event;
    }
}
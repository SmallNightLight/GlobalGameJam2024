using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [AddComponentMenu("GameEvent Listeners/WaveData Event Listener")]
    public class WaveDataGameEventListener : GameEventListenerBase<WaveData>
    {
        [SerializeField] private WaveDataVariable _event;

        public override IGameEvent<WaveData> GetGameEventT() => _event;
    }
}
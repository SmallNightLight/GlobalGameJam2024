using ScriptableArchitecture.Core;
using UnityEngine;

namespace ScriptableArchitecture.Data
{
    [AddComponentMenu("GameEvent Listeners/WorldTextMessage Event Listener")]
    public class WorldTextMessageGameEventListener : GameEventListenerBase<WorldTextMessage>
    {
        [SerializeField] private WorldTextMessageVariable _event;

        public override IGameEvent<WorldTextMessage> GetGameEventT() => _event;
    }
}
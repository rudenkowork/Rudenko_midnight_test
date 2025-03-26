using System;

namespace Gameplay.Infrastructure.Controllers
{
    [Serializable]
    public struct BroadcasterData
    {
        public BroadcastType TypeId;
        public Broadcaster Broadcaster;
    }
}
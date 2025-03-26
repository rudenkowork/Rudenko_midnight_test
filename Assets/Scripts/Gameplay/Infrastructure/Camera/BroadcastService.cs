using Gameplay.Infrastructure.Controllers;
using Services.EventBus;

namespace Gameplay.Infrastructure.Camera
{
    public class BroadcastService : IBroadcastService
    {
        private BroadcastType _broadcastType;

        public BroadcastType BroadcastType
        {
            get => _broadcastType;
            set
            {
                _broadcastType = value;
                GameplayEventBus.Instance.OnBroadcasterChanged?.Invoke(value);
            }
        }
    }
}
using Gameplay.Infrastructure.Controllers;

namespace Gameplay.Infrastructure.Camera
{
    public interface IBroadcastService
    {
        BroadcastType BroadcastType { get; set; }
    }
}
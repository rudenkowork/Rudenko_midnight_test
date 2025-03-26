using Configs;

namespace Gameplay.Food.Services
{
    public struct Order
    {
        public string UniqueId;
        public OrderData Data;

        public Order(string uniqueId, OrderData data)
        {
            UniqueId = uniqueId;
            Data = data;
        }
    }
}
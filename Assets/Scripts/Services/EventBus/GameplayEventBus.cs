using System;
using Gameplay.Common;
using Gameplay.Food.Services;
using Gameplay.Infrastructure.Controllers;

namespace Services.EventBus
{
    public class GameplayEventBus
    {
        private GameplayEventBus()
        {
        }

        public static GameplayEventBus Instance => _instance ??= new GameplayEventBus();
        
        private static GameplayEventBus _instance;

        public Action OnBalanceChanged;
        public Action<FoodType> OnFoodSelected;
        public Action<FoodType> OnProductPut;
        public Action<FoodType> OnProductTaken;
        public Action OnOrderPlaced;
        public Action OnOrderDone;
        public Action OnOrderEated;
        public Action<BroadcastType> OnBroadcasterChanged;
        public Action<Plate> OnPlateReady;
    }
}
using System;
using Gameplay.Food.Services;

namespace Configs
{
    [Serializable]
    public class SavedFood
    {
        public FoodType FoodType;
        public int Amount;
    }
}
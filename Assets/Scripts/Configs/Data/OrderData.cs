using System;
using System.Collections.Generic;
using Gameplay.Food.Services;

namespace Configs
{
    [Serializable]
    public struct OrderData
    {
        public List<FoodType> Foods;
        public int Cost;
    }
}
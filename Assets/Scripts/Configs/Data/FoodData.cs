using System;
using Gameplay.Food;
using Gameplay.Food.Services;
using UnityEngine;

namespace Configs
{
    [Serializable]
    public struct FoodData
    {
        public FoodType TypeId;
        public Sprite Sprite;
        public FoodBase Prefab;
    }
}
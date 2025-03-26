using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Gameplay.Food.Services;

namespace Services.DataManagement.ProgressData
{
    [Serializable]
    public class WalletData
    {
        public int Balance = 100;

        public List<SavedFood> Foods = Enum.GetValues(typeof(FoodType))
            .Cast<FoodType>()
            .Where(foodType => foodType != FoodType.UNKNOWN)
            .Select(foodType => new SavedFood
            {
                FoodType = foodType,
                Amount = foodType switch
                {
                    FoodType.BUNS => 1,
                    FoodType.SALAD => 1,
                    FoodType.RAW_TOMATO => 1,
                    FoodType.RAW_MEAT => 1,
                    FoodType.RAW_CROISSANT => 1,
                    FoodType.CHOPPED_POTATO => 1,
                    FoodType.RAW_POTATO => 1,
                    FoodType.RAW_CHEESE => 1,
                    _ => 0
                }
            })
            .ToList();
    }
}
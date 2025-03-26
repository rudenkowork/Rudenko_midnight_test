using System;
using Gameplay.Food.Services;

namespace Services.DataManagement.ProgressData
{
    [Serializable]
    public class AnalyticsData
    {
        public int BonusAmount;
        public FoodType SelectedFoodType = FoodType.UNKNOWN;
    }
}
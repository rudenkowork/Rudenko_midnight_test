using Gameplay.Food;
using Gameplay.Food.Services;
using Services.WindowsManagement.Windows;
using UnityEngine;

namespace Services.DataManagement
{
    public interface IStaticDataService
    {
        WindowBase GetWindow(WindowType windowType);
        Sprite GetFoodIcon(FoodType foodType);
        FoodBase GetFoodPrefab(FoodType foodType);
        Order GetRandomOrder(string orderNumber);
    }
}
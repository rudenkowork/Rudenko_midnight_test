using Gameplay.Common;
using UnityEngine;

namespace Gameplay.Food.Services
{
    public interface IFoodFactory
    {
        IngredientComponent CreateIngredientComponent(FoodType foodType, Transform parent);
        FoodBase CreateFood(FoodType foodType, Transform parent);
        PlateIngredientComponent CreateIconIngredient(Sprite getFoodIcon, Transform ingredientsParent);
        Plate CreatePlate(Order order, Transform parent, Camera cam);
    }
}
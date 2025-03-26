using Gameplay.Common;
using Services;
using Services.AssetManagement;
using Services.DataManagement;
using Services.DataManagement.ProgressData;
using UnityEngine;

namespace Gameplay.Food.Services
{
    public class FoodFactory : IFoodFactory
    {
        private readonly IInstantiator _instantiator;
        private readonly IStaticDataService _staticData;
        private readonly IAssetProvider _assetProvider;
        private readonly IUserData _userData;

        public FoodFactory(IAssetProvider assetProvider, IStaticDataService staticData, IInstantiator instantiator,
            IUserData userData)
        {
            _assetProvider = assetProvider;
            _staticData = staticData;
            _instantiator = instantiator;
            _userData = userData;
        }

        public IngredientComponent CreateIngredientComponent(FoodType foodType, Transform parent)
        {
            IngredientComponent prefab =
                _assetProvider.LoadResource<IngredientComponent>(AssetPath.IngredientComponentPath);
            
            IngredientComponent instance = _instantiator.InstantiatePrefabForComponent(prefab, parent);
            
            int amount = _userData.UserData.Wallet.Foods.Find(food => food.FoodType == foodType).Amount;
            Sprite sprite = _staticData.GetFoodIcon(foodType);
            bool isSelected = _userData.UserData.Analytics.SelectedFoodType == foodType;

            instance.Initialize(foodType, amount, sprite, isSelected);
            
            return instance;
        }

        public FoodBase CreateFood(FoodType foodType, Transform parent)
        {
            FoodBase prefab = _staticData.GetFoodPrefab(foodType);
            FoodBase instance = _instantiator.InstantiatePrefabForComponent(prefab, parent);
            
            return instance;
        }

        public PlateIngredientComponent CreateIconIngredient(Sprite getFoodIcon, Transform ingredientsParent)
        {
            var prefab = _assetProvider.LoadResource<PlateIngredientComponent>(AssetPath.PlateIngredientPath);
            var instance = _instantiator.InstantiatePrefabForComponent(prefab, ingredientsParent);
            
            instance.Icon.sprite = getFoodIcon;
            
            return instance;
        }

        public Plate CreatePlate(Order order, Transform parent, Camera cam)
        {
            Plate prefab = _assetProvider.LoadResource<Plate>(AssetPath.PlatePath);
            Plate instance = _instantiator.InstantiatePrefabForComponent(prefab, parent);
            
            instance.PutFood(order, cam);
            
            return instance;
        }
    }
}
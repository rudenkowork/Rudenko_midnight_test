using System.Collections.Generic;
using System.Linq;
using Configs;
using Gameplay.Food;
using Gameplay.Food.Services;
using Services.AssetManagement;
using Services.WindowsManagement.Windows;
using Tools.Extensions;
using UnityEngine;

namespace Services.DataManagement
{
    public class StaticDataService : IStaticDataService
    {
        private readonly IAssetProvider _assetProvider;

        private Dictionary<WindowType, WindowBase> _windows;
        private Dictionary<FoodType, FoodData> _foods;
        private List<OrderData> _orders;

        public StaticDataService(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;

            InitData();
        }

        public WindowBase GetWindow(WindowType windowType) =>
            _windows.GetValueOrDefault(windowType);

        public Sprite GetFoodIcon(FoodType foodType) =>
            _foods.GetValueOrDefault(foodType).Sprite;

        public FoodBase GetFoodPrefab(FoodType foodType) =>
            _foods.GetValueOrDefault(foodType).Prefab;

        public Order GetRandomOrder(string orderNumber)
        {
            var orders = _orders.Shuffle();
            OrderData data = orders.Random();
            string uniqueId = orderNumber;

            return new Order(uniqueId, data);
        } 

        private void InitData()
        {
            _windows = _assetProvider.LoadResource<WindowsConfig>(AssetPath.WindowsPath).Windows
                .ToDictionary(window => window.TypeId, window => window.Prefab);

            _foods = _assetProvider.LoadResource<FoodsConfig>(AssetPath.FoodsPath).Foods
                .ToDictionary(food => food.TypeId, food => food);

            _orders = _assetProvider.LoadResource<OrdersConfig>(AssetPath.OrdersPath).Orders;
        }
    }
}
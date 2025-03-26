using System;
using System.Collections.Generic;
using Gameplay.Food.Services;
using Services;
using Services.AssetManagement;
using Services.DataManagement;
using Services.EventBus;
using UnityEngine;

namespace Gameplay.Food.Orders
{
    public class OrdersService : IOrdersService
    {
        public List<Order> Orders { get; } = new();

        private readonly IStaticDataService _staticData;
        private readonly IInstantiator _instantiator;
        private readonly IAssetProvider _assetProvider;
        
        private int _orderNumber = 0;

        public OrdersService(IStaticDataService staticData, IInstantiator instantiator, IAssetProvider assetProvider)
        {
            _staticData = staticData;
            _instantiator = instantiator;
            _assetProvider = assetProvider;
        }
        
        public OrderComponent CreateOrderComponent(Order order, Transform parent)
        {
            var prefab = _assetProvider.LoadResource<OrderComponent>(AssetPath.OrderComponentPath);
            var instance = _instantiator.InstantiatePrefabForComponent(prefab, parent);
            
            instance.Initialize(order);

            return instance;
        }

        public void PlaceOrder()
        {
            _orderNumber++;
            Order order = _staticData.GetRandomOrder(_orderNumber.ToString());
            Orders.Add(order);

            GameplayEventBus.Instance.OnOrderPlaced?.Invoke();
        }

        public void RemoveOrder(string id)
        {
            int index = Orders.FindIndex(order => order.UniqueId == id);
            Orders.RemoveAt(index);
            _orderNumber--;
            
            GameplayEventBus.Instance.OnOrderDone?.Invoke();
        }
    }
}
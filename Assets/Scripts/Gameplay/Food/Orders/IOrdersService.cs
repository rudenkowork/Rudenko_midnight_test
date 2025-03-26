using System;
using System.Collections.Generic;
using Gameplay.Food.Orders;
using UnityEngine;

namespace Gameplay.Food.Services
{
    public interface IOrdersService
    {
        List<Order> Orders { get; }
        void PlaceOrder();
        OrderComponent CreateOrderComponent(Order order, Transform parent);
        void RemoveOrder(string id);
    }
}
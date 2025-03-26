using System.Collections;
using System.Collections.Generic;
using Gameplay.Food.Orders;
using Gameplay.Food.Services;
using Plugins.RContainer;
using UnityEngine;

namespace Services.WindowsManagement.Windows
{
    public class OrdersWindow : WindowBase
    {
        public Transform OrdersParent;

        private List<OrderComponent> _orders = new();
        private IOrdersService _ordersService;
        private IWindowService _windowService;

        [Inject]
        private void Construct(IOrdersService ordersService, IWindowService windowService)
        {
            _ordersService = ordersService;
            _windowService = windowService;
        }

        protected override void CloseWindow()
        {
            base.CloseWindow();
            _windowService.CloseWindow();
        }

        protected override void OnStartAction()
        {
            base.OnStartAction();
            foreach (var order in _ordersService.Orders)
            {
                _ordersService.CreateOrderComponent(order, OrdersParent);
            }
        }
    }
}
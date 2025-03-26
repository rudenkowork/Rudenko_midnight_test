using Gameplay.Food.Services;
using Plugins.RContainer;
using Services.EventBus;
using Tools.Extensions;
using UnityEngine;

namespace Gameplay.Food.Orders
{
    public class OrdersManager : MonoBehaviour
    {
        public Transform PlateParent;
        public UnityEngine.Camera ThisCamera;

        private IOrdersService _ordersService;
        private IFoodFactory _foodFactory;

        [Inject]
        private void Construct(IOrdersService ordersService, IFoodFactory foodFactory)
        {
            _foodFactory = foodFactory;
            _ordersService = ordersService;
        }

        private void OnEnable()
        {
            GameplayEventBus.Instance.OnOrderPlaced += PlaceOrder;
        }

        private void OnDisable()
        {
            GameplayEventBus.Instance.OnOrderPlaced -= PlaceOrder;
        }

        private void PlaceOrder()
        {
            if (!_ordersService.Orders.IsEmpty())
            {
                _foodFactory.CreatePlate(_ordersService.Orders[0], PlateParent, ThisCamera);
            }
        }
    }
}
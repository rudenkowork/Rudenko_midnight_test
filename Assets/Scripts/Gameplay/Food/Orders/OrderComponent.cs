using Gameplay.Food.Services;
using Plugins.RContainer;
using Services.DataManagement;
using TMPro;
using UnityEngine;

namespace Gameplay.Food.Orders
{
    public class OrderComponent : MonoBehaviour
    {
        public Transform IngredientsParent;
        public TextMeshProUGUI OrderNumber;

        private string _uniqueId;
        private IFoodFactory _foodFactory;
        private IStaticDataService _staticData;

        [Inject]
        private void Construct(IFoodFactory foodFactory, IStaticDataService staticData)
        {
            _staticData = staticData;
            _foodFactory = foodFactory;
        }

        public void Initialize(Order order)
        {
            _uniqueId = order.UniqueId;
            OrderNumber.text = $"Order {_uniqueId}";

            foreach (FoodType foodType in order.Data.Foods)
            {
                _foodFactory.CreateIconIngredient(_staticData.GetFoodIcon(foodType), IngredientsParent);
            }
        }
    }
}
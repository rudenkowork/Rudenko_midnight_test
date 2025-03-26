using System.Collections;
using Gameplay.Food;
using Gameplay.Food.Services;
using Gameplay.Infrastructure.Controllers;
using Plugins.RContainer;
using Services.EventBus;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Common
{
    public class Stove : MonoBehaviour
    {
        [Header("References")] public Transform PanPosition;
        public Transform PotPosition;
        public Button CookButton;

        [Header("Cooking Times")] public float CookDuration = 5f;

        private IFoodService _foodService;
        private IFoodFactory _foodFactory;

        private FoodBase _meatProduct;
        private bool _isMeatCooking;
        private bool _isMeatCooked;

        private FoodBase _potatoProduct;
        private bool _isPotatoCooking;
        private bool _isPotatoCooked;

        [Inject]
        private void Construct(IFoodService foodService, IFoodFactory foodFactory)
        {
            _foodService = foodService;
            _foodFactory = foodFactory;
        }

        private void OnEnable()
        {
            CookButton.onClick.AddListener(OnCookButtonClicked);

            GameplayEventBus.Instance.OnBroadcasterChanged += HandleUI;
        }

        private void OnDisable()
        {
            CookButton.onClick.RemoveListener(OnCookButtonClicked);
            
            GameplayEventBus.Instance.OnBroadcasterChanged -= HandleUI;
        }

        private void HandleUI(BroadcastType broadcastType)
        {
            CookButton.gameObject.SetActive(broadcastType == BroadcastType.STOVE);
        }

        private void OnCookButtonClicked()
        {
            FoodType selected = _foodService.SelectedFood;
            Debug.Log(selected);
            
            if (_isMeatCooked && selected == FoodType.UNKNOWN)
            {
                _foodService.Take(FoodType.COOKED_MEAT);
                Destroy(_meatProduct.gameObject);
                _meatProduct = null;
                _isMeatCooked = false;
                return;
            }
            
            if (_isPotatoCooked && selected == FoodType.UNKNOWN)
            {
                _foodService.Take(FoodType.FRIES);
                Destroy(_potatoProduct.gameObject);
                _potatoProduct = null;
                _isPotatoCooked = false;
                return;
            }

            if (selected == FoodType.RAW_MEAT)
            {
                if (_meatProduct == null && !_isMeatCooking)
                {
                    _meatProduct = _foodFactory.CreateFood(FoodType.RAW_MEAT, PanPosition);
                    _foodService.Put(FoodType.RAW_MEAT);

                    StartCoroutine(CookMeatRoutine());
                }

                return;
            }

            if (selected == FoodType.CHOPPED_POTATO)
            {
                if (_potatoProduct == null && !_isPotatoCooking)
                {
                    _potatoProduct = _foodFactory.CreateFood(FoodType.CHOPPED_POTATO, PotPosition);

                    _foodService.Put(FoodType.CHOPPED_POTATO);

                    StartCoroutine(CookPotatoRoutine());
                }

                return;
            }
        }

        private IEnumerator CookMeatRoutine()
        {
            _isMeatCooking = true;

            yield return new WaitForSeconds(CookDuration);

            if (_meatProduct != null)
            {
                Destroy(_meatProduct.gameObject);
                _meatProduct = null;

                _meatProduct = _foodFactory.CreateFood(FoodType.COOKED_MEAT, PanPosition);

                _isMeatCooked = true;
            }

            _isMeatCooking = false;
        }

        private IEnumerator CookPotatoRoutine()
        {
            _isPotatoCooking = true;

            yield return new WaitForSeconds(CookDuration);

            if (_potatoProduct != null)
            {
                Destroy(_potatoProduct.gameObject);
                _potatoProduct = null;

                _potatoProduct = _foodFactory.CreateFood(FoodType.FRIES, PotPosition);

                _isPotatoCooked = true;
            }

            _isPotatoCooking = false;
        }
    }
}
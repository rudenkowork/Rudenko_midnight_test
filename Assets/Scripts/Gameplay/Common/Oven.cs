using System;
using System.Collections;
using Gameplay.Food;
using Gameplay.Food.Services;
using Plugins.RContainer;
using UnityEngine;
using UnityEngine.UI;
using Gameplay.Infrastructure.Camera;
using Gameplay.Infrastructure.Controllers;
using Services.EventBus;
using TMPro; // Assuming BroadcastService and BroadcastType live here

namespace Gameplay.Common
{
    public class Oven : MonoBehaviour
    {
        public Transform Door;
        public Transform FoodPosition;
        public Button CookButton;

        private bool _isOpen;
        private Coroutine _currentCoroutine;
        private IFoodService _foodService;
        private IFoodFactory _foodFactory;
        private IBroadcastService _broadcastService;
        private FoodBase _cookingProduct;
        private FoodType _cookingType;
        private bool _isCooking;
        private bool _isCooked;
        private FoodType _cookedType;

        [Inject]
        private void Construct(IFoodService foodService, IFoodFactory foodFactory, IBroadcastService broadcastService)
        {
            _foodService = foodService;
            _foodFactory = foodFactory;
            _broadcastService = broadcastService;
        }

        private void OnEnable()
        {
            CookButton.onClick.AddListener(Cook);
            
            GameplayEventBus.Instance.OnBroadcasterChanged += HandleUI;
        }

        private void OnDisable()
        {
            CookButton.onClick.RemoveListener(Cook);
            
            GameplayEventBus.Instance.OnBroadcasterChanged -= HandleUI;
        }

        private void HandleUI(BroadcastType broadcastType)
        {
            CookButton.gameObject.SetActive(broadcastType == BroadcastType.OVEN);
        }

        public void Open()
        {
            if (_isOpen || _isCooking) return;
            if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
            
            _currentCoroutine = StartCoroutine(RotateDoor(80f));
            _isOpen = true;
        }

        public void Close()
        {
            if (!_isOpen) return;
            if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);

            _currentCoroutine = StartCoroutine(RotateDoor(0f));
            _isOpen = false;
        }

        private void Cook()
        {
            if (_isCooked && _foodService.SelectedFood == FoodType.UNKNOWN)
            {
                _foodService.Take(_cookedType);
                _cookedType = FoodType.UNKNOWN;
                if (_cookingProduct != null)
                {
                    Destroy(_cookingProduct.gameObject);
                    _cookingProduct = null;
                }
                _isCooked = false;
                return;
            }

            if (_foodService.SelectedFood is FoodType.RAW_CROISSANT or FoodType.RAW_MEAT && _cookingProduct == null)
            {
                _cookingProduct = _foodFactory.CreateFood(_foodService.SelectedFood, FoodPosition);
                _cookingType = _foodService.SelectedFood;
                _foodService.Put(_foodService.SelectedFood);

                StartCoroutine(CookRoutine());
            }
        }

        private IEnumerator CookRoutine()
        {
            Close();
            _isCooking = true;
            yield return new WaitForSeconds(10f);

            if (_cookingProduct != null)
            {
                Destroy(_cookingProduct.gameObject);
                _cookingProduct = null;
                _cookedType = _cookingType == FoodType.RAW_MEAT ? FoodType.COOKED_MEAT : FoodType.CROISSANT;
                _cookingProduct =
                    _foodFactory.CreateFood(
                        _cookedType, FoodPosition
                    );
            }

            _isCooking = false;
            _isCooked = true;
            
            if (_broadcastService.BroadcastType == BroadcastType.OVEN)
            {
                Open();
            }
        }

        private IEnumerator RotateDoor(float targetX)
        {
            Quaternion startRotation = Door.rotation;
            Quaternion endRotation = Quaternion.Euler(
                targetX,
                Door.rotation.eulerAngles.y,
                Door.rotation.eulerAngles.z
            );

            float duration = 0.6f;
            float time = 0f;

            while (time < duration)
            {
                Door.rotation = Quaternion.Slerp(startRotation, endRotation, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            Door.rotation = endRotation;
            Debug.Log(endRotation.eulerAngles);
        }
    }
}
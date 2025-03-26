using System;
using System.Collections;
using Gameplay.Food.Services;
using Plugins.RContainer;
using Services.DataManagement;
using Services.DataManagement.ProgressData;
using Services.EventBus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Food
{
    public class IngredientComponent : MonoBehaviour
    {
        public Image Ingredient;
        public TextMeshProUGUI Amount;

        private IUserData _userData;
        private ISaveLoadService _saveLoadService;
        private FoodType _foodType = FoodType.UNKNOWN;
        private Button _button;
        private bool _isSelected;
        private int _amount;
        private Coroutine _scaleRoutine;
        private IFoodService _foodService;

        [Inject]
        private void Construct(IUserData userData, ISaveLoadService saveLoadService, IFoodService foodService)
        {
            _saveLoadService = saveLoadService;
            _userData = userData;
            _foodService = foodService;
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(Select);
            GameplayEventBus.Instance.OnFoodSelected += Deselect;
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(Select);
            GameplayEventBus.Instance.OnFoodSelected -= Deselect;
        }

        public void Initialize(FoodType foodType, int amount, Sprite sprite, bool isSelected)
        {
            _isSelected = isSelected;
            if (_isSelected) Select();
            _foodType = foodType;
            _amount = amount;
            if (_amount == 0) Disable();
            Ingredient.sprite = sprite;
            Amount.text = amount.ToString();
        }

        private void Disable()
        {
            if (_userData.UserData.Analytics.SelectedFoodType == _foodType)
            {
                _userData.UserData.Analytics.SelectedFoodType = FoodType.UNKNOWN;
                _saveLoadService.Update();
            }

            _button.interactable = false;
            _button.image.color = new Color(255f, 255f, 255f, 0.5f);
            
            if (_scaleRoutine != null) StopCoroutine(_scaleRoutine);
            _scaleRoutine = StartCoroutine(ScaleIngredient(new Vector3(1f, 1f, 1f), 0.05f));
        }

        private void Select()
        {
            if (_isSelected)
            {
                _foodService.SelectedFood = FoodType.UNKNOWN;
                _isSelected = false;
                if (_scaleRoutine != null) StopCoroutine(_scaleRoutine);
                _scaleRoutine = StartCoroutine(ScaleIngredient(new Vector3(1f, 1f, 1f), 0.05f));

                GameplayEventBus.Instance.OnFoodSelected?.Invoke(FoodType.UNKNOWN);

                return;
            }

            _foodService.SelectedFood = _foodType;
            
            _isSelected = true;

            if (_scaleRoutine != null) StopCoroutine(_scaleRoutine);
            _scaleRoutine = StartCoroutine(ScaleIngredient(new Vector3(1.2f, 1.2f, 1f), 0.05f));

            GameplayEventBus.Instance.OnFoodSelected?.Invoke(_foodType);
        }

        private void Deselect(FoodType foodType)
        {
            if (foodType == _foodType || !_isSelected) return;

            _button.interactable = true;
            _isSelected = false;

            if (_scaleRoutine != null) StopCoroutine(_scaleRoutine);
            _scaleRoutine = StartCoroutine(ScaleIngredient(new Vector3(1f, 1f, 1f), 0.05f));
        }

        private IEnumerator ScaleIngredient(Vector3 targetScale, float duration)
        {
            Vector3 initialScale = Ingredient.transform.localScale;
            float time = 0f;

            while (time < duration)
            {
                Ingredient.transform.localScale = Vector3.Lerp(initialScale, targetScale, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            Ingredient.transform.localScale = targetScale;
        }
    }
}
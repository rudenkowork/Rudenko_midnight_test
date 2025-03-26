using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Gameplay.Food.Services;
using Plugins.RContainer;
using Services.DataManagement;
using Services.DataManagement.ProgressData;
using Services.EventBus;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Common
{
    public class Plate : MonoBehaviour
    {
        public Transform IngredientsParent;
        public Button PutIngredientButton;
        public Canvas IngredientsCanvas;

        public GameObject Burger;
        public GameObject Fries;
        public GameObject Croissant;

        private List<FoodType> _foodTypes = new();
        private IFoodService _foodService;
        private IFoodFactory _foodFactory;
        private IStaticDataService _staticData;
        private int _cost;
        private IUserData _userData;
        private ISaveLoadService _saveLoadService;
        private Camera _camera;
        private Dictionary<FoodType, PlateIngredientComponent> _ingredients = new();
        private List<FoodType> _recipe;
        private IOrdersService _ordersService;
        private string _id;

        [Inject]
        private void Construct(
            IFoodService foodService,
            IFoodFactory foodFactory,
            IStaticDataService staticData,
            IUserData userData,
            ISaveLoadService saveLoadService,
            IOrdersService ordersService
        )
        {
            _saveLoadService = saveLoadService;
            _foodService = foodService;
            _foodFactory = foodFactory;
            _staticData = staticData;
            _userData = userData;
            _ordersService = ordersService;
        }

        private void OnEnable()
        {
            PutIngredientButton.onClick.AddListener(PutIngredient);
        }

        private void OnDisable()
        {
            PutIngredientButton.onClick.RemoveListener(PutIngredient);
        }

        private void Start()
        {
            IngredientsCanvas.worldCamera = _camera;
        }

        public void PutFood(Order order, Camera cam)
        {
            _id = order.UniqueId;
            var newFoodTypes = new List<FoodType>();
            foreach (var foodType in order.Data.Foods)
            {
                if (foodType == FoodType.BURGER)
                {
                    newFoodTypes.Add(FoodType.SLICED_CHEESE);
                    newFoodTypes.Add(FoodType.SLICED_TOMATO);
                    newFoodTypes.Add(FoodType.SALAD);
                    newFoodTypes.Add(FoodType.BUNS);
                    newFoodTypes.Add(FoodType.COOKED_MEAT);
                }
                else
                {
                    newFoodTypes.Add(foodType);
                }
            }

            _foodTypes = newFoodTypes;
            _recipe = new List<FoodType>(_foodTypes);
            _cost = order.Data.Cost;

            foreach (var foodType in _foodTypes)
            {
                _ingredients[foodType] =
                    _foodFactory.CreateIconIngredient(_staticData.GetFoodIcon(foodType), IngredientsParent);
            }

            _camera = cam;
        }

        private void PutIngredient()
        {
            // Ищем первый ингредиент из _foodTypes, который есть у игрока
            FoodType available = FoodType.UNKNOWN;

            foreach (var foodType in _foodTypes)
            {
                if (_userData.UserData.Wallet.Foods.Any(food => food.FoodType == foodType && food.Amount > 0))
                {
                    available = foodType;
                    break;
                }
            }

            // Если ничего не найдено — просто выходим
            if (available == FoodType.UNKNOWN)
                return;

            // Удаляем иконку ингредиента с тарелки
            if (_ingredients.ContainsKey(available))
            {
                Destroy(_ingredients[available].gameObject);
                _ingredients.Remove(available);
            }

            _userData.UserData.Wallet.Foods.Find(food => food.FoodType == available).Amount--;
            _saveLoadService.Update();

            _foodTypes.Remove(available);

            GameplayEventBus.Instance.OnProductPut?.Invoke(available);

            if (_foodTypes.Count == 0)
            {
                FinalizeRecipe();
            }
        }

        private void FinalizeRecipe()
        {
            if (_recipe.Contains(FoodType.CROISSANT))
            {
                Croissant.SetActive(true);
            }

            if (_recipe.Contains(FoodType.FRIES))
            {
                Fries.SetActive(true);
            }

            if (_recipe.Contains(FoodType.BUNS))
            {
                Burger.SetActive(true);
            }

            IngredientsCanvas.gameObject.SetActive(false);

            _userData.UserData.Wallet.Balance += _cost;
            _saveLoadService.Update();

            GameplayEventBus.Instance.OnBalanceChanged?.Invoke();

            StartCoroutine(FadeOutPlate());
        }

        private IEnumerator FadeOutPlate()
        {
            yield return new WaitForSeconds(1f);
            GameplayEventBus.Instance.OnPlateReady?.Invoke(this);
        }

        public void StartEating()
        {
            StartCoroutine(Eat());
        }

        public IEnumerator Eat()
        {
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);

            GameplayEventBus.Instance.OnOrderDone?.Invoke();
            _ordersService.RemoveOrder(_id);
            GameplayEventBus.Instance.OnOrderEated?.Invoke();
        }
    }
}
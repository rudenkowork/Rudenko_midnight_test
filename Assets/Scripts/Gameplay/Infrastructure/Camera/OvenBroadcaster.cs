using Gameplay.Common;
using Gameplay.Food;
using Gameplay.Food.Services;
using Plugins.RContainer;
using Services.DataManagement;
using Services.DataManagement.ProgressData;
using Services.EventBus;
using UnityEngine;

namespace Gameplay.Infrastructure.Camera
{
    public class OvenBroadcaster : Broadcaster
    {
        public Oven Oven;
        public Transform FoodParent;

        private IFoodFactory _foodFactory;
        private IUserData _userData;
        private FoodBase _takenFood;
        private ISaveLoadService _saveLoadService;
        private IFoodService _foodService;

        [Inject]
        private void Construct(IFoodFactory foodFactory, IUserData userData, ISaveLoadService saveLoadService,
            IFoodService foodService)
        {
            _foodService = foodService;
            _userData = userData;
            _foodFactory = foodFactory;
            _saveLoadService = saveLoadService;
        }

        public override void Enable()
        {
            base.Enable();
            Oven.Open();

            GameplayEventBus.Instance.OnFoodSelected += HandleProduct;
            GameplayEventBus.Instance.OnProductPut += OnProductPut;
            GameplayEventBus.Instance.OnProductTaken += OnProductTaken;

            HandleFood(true);
        }

        public override void Disable()
        {
            base.Disable();
            Oven.Close();

            GameplayEventBus.Instance.OnFoodSelected -= HandleProduct;
            GameplayEventBus.Instance.OnProductPut -= OnProductPut;
            GameplayEventBus.Instance.OnProductTaken -= OnProductTaken;

            HandleFood(false);
        }
        
        private void OnProductPut(FoodType foodType)
        {
            // if (_userData.UserData.Wallet.Foods.Find(food => food.FoodType == foodType).Amount == 0)
            // {
                ClearTakenProduct();
                _foodService.SelectedFood = FoodType.UNKNOWN;
            // }
        }

        private void OnProductTaken(FoodType foodType)
        {
            if (_takenFood == null)
            {
                _foodService.SelectedFood = foodType;

                _takenFood = _foodFactory.CreateFood(_foodService.SelectedFood, FoodParent);
            }
        }

        private void HandleProduct(FoodType foodType)
        {
            HandleFood(gameObject.activeSelf);
        }

        private void HandleFood(bool isActive)
        {
            ClearTakenProduct();

            if (isActive)
            {
                if (_foodService.SelectedFood is FoodType.RAW_MEAT or FoodType.RAW_CROISSANT)
                {
                    _takenFood = _foodFactory.CreateFood(_foodService.SelectedFood, FoodParent);
                }
            }
        }

        private void ClearTakenProduct()
        {
            if (_takenFood != null)
            {
                Destroy(_takenFood.gameObject);
                _takenFood = null;
            }
        }
    }
}
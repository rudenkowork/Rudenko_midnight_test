using Plugins.RContainer;
using Services.EventBus;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Configs;
using Gameplay.Food.Services;
using Services.DataManagement;
using Services.DataManagement.ProgressData;

namespace Services.WindowsManagement.Windows
{
    public class ShopWindow : WindowBase
    {
        public Button BuyBunsButton;
        public Button BuyMeatButton;
        public Button BuySaladButton;
        public Button BuyTomatoButton;
        public Button BuyCheeseButton;
        public Button BuyPotatoButton;
        public Button BuyCroissantButton;

        public Button BalanceButton;
        
        private IWindowService _windowService;
        private IUserData _userData;
        private ISaveLoadService _saveLoad;

        private readonly Dictionary<FoodType, (Button button, int price)> _shopData = new();

        [Inject]
        private void Construct(IWindowService windowService, IUserData userData, ISaveLoadService saveLoad)
        {
            _windowService = windowService;
            _userData = userData;
            _saveLoad = saveLoad;
        }

        private void Awake()
        {
            BalanceButton.interactable = false;
            
            _shopData.Add(FoodType.BUNS, (BuyBunsButton, 20));
            _shopData.Add(FoodType.COOKED_MEAT, (BuyMeatButton, 50));
            _shopData.Add(FoodType.SALAD, (BuySaladButton, 10));
            _shopData.Add(FoodType.SLICED_TOMATO, (BuyTomatoButton, 10));
            _shopData.Add(FoodType.SLICED_CHEESE, (BuyCheeseButton, 15));
            _shopData.Add(FoodType.RAW_POTATO, (BuyPotatoButton, 20));
            _shopData.Add(FoodType.RAW_CROISSANT, (BuyCroissantButton, 25));

            foreach (var pair in _shopData)
            {
                var foodType = pair.Key;
                var button = pair.Value.button;
                var price = pair.Value.price;

                button.onClick.AddListener(() => Buy(foodType, price));
            }
        }

        protected override void OnEnableAction()
        {
            base.OnEnableAction();
            GameplayEventBus.Instance.OnBalanceChanged += UpdateButtons;
        }
        
        protected override void OnDisableAction()
        {
            base.OnDisableAction();
            GameplayEventBus.Instance.OnBalanceChanged -= UpdateButtons;
        }
        
        protected override void OnStartAction()
        {
            base.OnStartAction();
            UpdateButtons();
        }

        protected override void CloseWindow()
        {
            base.CloseWindow();
            _windowService.CloseWindow();
        }

        private void Buy(FoodType foodType, int price)
        {
            var wallet = _userData.UserData.Wallet;

            if (wallet.Balance < price)
                return;

            wallet.Balance -= price;

            var foodItem = wallet.Foods.Find(f => f.FoodType == foodType);
            if (foodItem != null)
                foodItem.Amount++;
            else
                wallet.Foods.Add(new SavedFood() { FoodType = foodType, Amount = 1 });

            _saveLoad.Update();
            GameplayEventBus.Instance.OnBalanceChanged?.Invoke();
        }

        private void UpdateButtons()
        {
            var balance = _userData.UserData.Wallet.Balance;
            foreach (var pair in _shopData)
            {
                pair.Value.button.interactable = balance >= pair.Value.price;
            }
        }
    }
}
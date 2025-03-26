using System;
using System.Linq;
using Gameplay.Food.Services;
using Plugins.RContainer;
using Services.DataManagement;
using Services.SceneManagement;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Services.WindowsManagement.Windows
{
    public class InventoryWindow : WindowBase
    {
        private const string MenuScene = "Menu";

        public Button HomeButton;
        public Transform IngredientsParent;

        private ISceneLoader _sceneLoader;
        private IStaticDataService _staticData;
        private IFoodFactory _foodFactory;
        private IWindowService _windowService;

        [Inject]
        private void Construct(
            ISceneLoader sceneLoader,
            IStaticDataService staticData,
            IFoodFactory foodFactory,
            IWindowService windowService
        )
        {
            _windowService = windowService;
            _foodFactory = foodFactory;
            _staticData = staticData;
            _sceneLoader = sceneLoader;
        }

        protected override void OnEnableAction()
        {
            base.OnEnableAction();
            HomeButton.onClick.AddListener(ToMenu);
        }

        protected override void OnDisableAction()
        {
            base.OnDisableAction();
            HomeButton.onClick.RemoveListener(ToMenu);
        }

        protected override void OnStartAction()
        {
            base.OnStartAction();
            CreateFoods();
        }

        protected override void CloseWindow()
        {
            base.CloseWindow();
            _windowService.CloseWindow();
        }

        private void CreateFoods()
        {
            foreach (FoodType food in Enum.GetValues(typeof(FoodType)).Cast<FoodType>().Skip(1))
            {
                _foodFactory.CreateIngredientComponent(food, IngredientsParent);
            }
        }

        private void ToMenu()
        {
            _sceneLoader.Load(MenuScene);
        }
    }
}
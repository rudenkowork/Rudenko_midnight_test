using System;
using Configs;
using Gameplay.Food.Services;
using Gameplay.Infrastructure.Controllers;
using Plugins.RContainer;
using Services.DataManagement;
using Services.DataManagement.ProgressData;
using Services.EventBus;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Common
{
    public class FridgeProduct : MonoBehaviour
    {
        public FoodType TypeId;
        public TextMeshProUGUI Amount;
        public Button HandleButton;

        private IUserData _userData;
        private SavedFood _inventoryFood;
        private SavedFood _fridgeFood;
        private ISaveLoadService _saveLoadService;
        private IFoodService _foodService;

        [Inject]
        private void Construct(IUserData userData, ISaveLoadService saveLoadService, IFoodService foodService)
        {
            _userData = userData;
            _saveLoadService = saveLoadService;
            _foodService = foodService;
        }

        private void OnEnable()
        {
            HandleButton.onClick.AddListener(HandleProduct);

            GameplayEventBus.Instance.OnBroadcasterChanged += HandleUI;
        }

        private void OnDisable()
        {
            HandleButton.onClick.RemoveListener(HandleProduct);

            GameplayEventBus.Instance.OnBroadcasterChanged -= HandleUI;
        }

        private void HandleUI(BroadcastType broadcastType)
        {
            HandleButton.gameObject.SetActive(broadcastType == BroadcastType.FRIDGE);
        }

        private void Start()
        {
            _inventoryFood = _userData.UserData.Wallet.Foods.Find(food => food.FoodType == TypeId);
            _fridgeFood = _userData.UserData.Fridge.Foods.Find(food => food.FoodType == TypeId);

            CheckAmount();
            SetAmount();
        }

        private void HandleProduct()
        {
            if (_foodService.SelectedFood == TypeId) PutProduct();
            else TakeProduct();
        }

        private void PutProduct()
        {
            if (_foodService.SelectedFood == TypeId && _inventoryFood.Amount > 0)
            {
                _inventoryFood.Amount--;
                _fridgeFood.Amount++;
                _saveLoadService.Update();

                CheckAmount();
                SetAmount();

                GameplayEventBus.Instance.OnProductPut?.Invoke(_foodService.SelectedFood);
            }
        }

        private void TakeProduct()
        {
            if (_foodService.SelectedFood == TypeId ||
                _foodService.SelectedFood == FoodType.UNKNOWN &&
                _fridgeFood.Amount > 0
               )
            {
                _inventoryFood.Amount++;
                _fridgeFood.Amount--;
                _saveLoadService.Update();

                CheckAmount();
                SetAmount();

                GameplayEventBus.Instance.OnProductTaken?.Invoke(TypeId);
            }
        }

        private void CheckAmount()
        {
            SetTransparency(_fridgeFood.Amount == 0 ? 0.5f : 1f);
        }

        private void SetAmount()
        {
            Amount.text = _fridgeFood.Amount.ToString();
        }

        private void SetTransparency(float alpha)
        {
            var renderers = gameObject.GetComponentsInChildren<Renderer>();
            foreach (var rend in renderers)
            {
                foreach (var mat in rend.materials)
                {
                    Color color = mat.color;
                    color.a = alpha;
                    mat.color = color;

                    if (mat.HasProperty("_Surface"))
                        mat.SetFloat("_Surface", 1);

                    mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    mat.SetInt("_ZWrite", 0);
                    mat.DisableKeyword("_ALPHATEST_ON");
                    mat.EnableKeyword("_ALPHABLEND_ON");
                    mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
                }
            }
        }
    }
}
using Services.DataManagement;
using Services.DataManagement.ProgressData;
using Services.EventBus;

namespace Gameplay.Food.Services
{
    public class FoodService : IFoodService
    {
        public FoodType SelectedFood
        {
            get => _userData.UserData.Analytics.SelectedFoodType;
            set
            {
                _userData.UserData.Analytics.SelectedFoodType = value;
                _saveLoadService.Update();
            }
        }

        private readonly IUserData _userData;
        private readonly ISaveLoadService _saveLoadService;

        public FoodService(IUserData userData, ISaveLoadService saveLoadService)
        {
            _userData = userData;
            _saveLoadService = saveLoadService;
        }

        public void Put(FoodType foodType)
        {
            _userData.UserData.Wallet.Foods.Find(food => food.FoodType == foodType).Amount--;
            _saveLoadService.Update();
            
            GameplayEventBus.Instance.OnProductPut?.Invoke(foodType);
        }

        public void Take(FoodType foodType)
        {
            _userData.UserData.Wallet.Foods.Find(food => food.FoodType == foodType).Amount++;
            _saveLoadService.Update();
            
            GameplayEventBus.Instance.OnProductTaken?.Invoke(foodType);
        }
    }
}
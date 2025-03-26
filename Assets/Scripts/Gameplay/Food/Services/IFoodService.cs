namespace Gameplay.Food.Services
{
    public interface IFoodService
    {
        FoodType SelectedFood { get; set; }
        void Put(FoodType foodType);
        void Take(FoodType cookedType);
    }
}
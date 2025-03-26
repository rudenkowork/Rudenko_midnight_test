using Gameplay.Food.Services;
using Plugins.RContainer;
using Services.DataManagement;
using Services.Factories;
using Services.Factories.Interfaces;
using Services.WindowsManagement;

namespace Installers
{
    public class UIInstaller : RInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IWindowFactory, WindowFactory>();
            Container.Bind<IWindowService, WindowService>();
            Container.Bind<IFoodFactory, FoodFactory>();
        }
    }
}
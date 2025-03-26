using Gameplay.Food.Orders;
using Gameplay.Food.Services;
using Gameplay.Infrastructure.Camera;
using Plugins.RContainer;
using Services;
using Services.AssetManagement;
using Services.DataManagement;
using Services.DataManagement.ProgressData;
using Services.SceneManagement;

namespace Installers
{
    public class ProjectInstaller : RInstaller, ICoroutineRunner
    {
        public override void InstallBindings()
        {
            Container.BindInstance<ICoroutineRunner>(this);
            Container.Bind<ISceneLoader, SceneLoader>();
            Container.Bind<IAssetProvider, AssetProvider>();
            Container.Bind<IStaticDataService, StaticDataService>();
            Container.Bind<IInstantiator, Instantiator>();
            Container.Bind<IUserData, UserDataService>();
            Container.Bind<ISaveLoadService, SaveLoadService>();
            Container.Bind<IFoodService, FoodService>();
            Container.Bind<IBroadcastService, BroadcastService>();
            Container.Bind<IOrdersService, OrdersService>();
        }
    }
}
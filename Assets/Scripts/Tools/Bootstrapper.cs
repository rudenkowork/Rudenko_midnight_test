using Plugins.RContainer;
using Services.DataManagement;
using Services.DataManagement.ProgressData;
using Services.SceneManagement;
using UnityEngine;

namespace Tools
{
    public class Bootstrapper : MonoBehaviour
    {
        private const string MenuScene = "Menu";

        private ISceneLoader _sceneLoader;
        private IUserData _userData;
        private ISaveLoadService _saveLoadService;

        [Inject]
        private void Construct(
            ISceneLoader sceneLoader,
            IUserData userData,
            ISaveLoadService saveLoadService
        )
        {
            _sceneLoader = sceneLoader;
            _userData = userData;
            _saveLoadService = saveLoadService;
        }

        private void Start()
        {
            SetData();
            _sceneLoader.InitialLoad(MenuScene);
        }

        private void SetData() =>
            _userData.UserData = _saveLoadService.LoadProgress() ?? _saveLoadService.InitialLoad();
    }
}

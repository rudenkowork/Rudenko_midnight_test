using System;

namespace Services.SceneManagement
{
    public interface IInitialSceneLoader
    {
        void InitialLoad(string sceneName, Action onLoaded = null);
    }
}
using System;

namespace Services.SceneManagement
{
    public interface ISceneLoader : IInitialSceneLoader
    {
        void Load(string name, Action onLoaded = null);
        void Reload(Action onLoaded = null);
    }
}
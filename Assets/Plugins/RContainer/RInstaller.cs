using UnityEngine;

namespace Plugins.RContainer
{
    public abstract class RInstaller : MonoBehaviour
    {
        protected Container Container { get; private set; }
        
        public void Activate()
        {
            Container = Container.GlobalInstance;

            InstallBindings();
        }
        
        public abstract void InstallBindings();
    }
}
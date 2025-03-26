using Plugins.RContainer;
using UnityEngine;

namespace Services
{
    public class Instantiator : IInstantiator
    {
        public TInstance InstantiatePrefabForComponent<TInstance>(TInstance prefab, Transform parent = null) where TInstance : MonoBehaviour
        {
            TInstance instance = Object.Instantiate(prefab, parent);
            Container.GlobalInstance.Inject(instance);

            return instance;
        }
    }
}
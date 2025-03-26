using System;
using UnityEngine;

namespace Plugins.RContainer
{
    public class SceneInjector : MonoBehaviour
    {
        private void Awake()
        {
            var monoBehaviours = FindObjectsOfType<MonoBehaviour>(includeInactive: true);
            foreach (var mb in monoBehaviours)
            {
                Container.GlobalInstance.Inject(mb);
            }
        }
    }
}
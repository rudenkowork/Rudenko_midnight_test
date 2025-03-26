using System;
using System.Collections.Generic;
using UnityEngine;

namespace Plugins.RContainer
{
    [DefaultExecutionOrder(-100)]
    public class ProjectContext : MonoBehaviour
    {
        public List<RInstaller> Installers = new();

        private void Awake()
        {
            DontDestroyOnLoad(this);
            SetupContainer();
        }

        private void SetupContainer()
        {
            foreach (RInstaller installer in Installers)
            {
                installer.Activate();
            }

            InjectAllMonoBehaviours();
        }

        private void InjectAllMonoBehaviours()
        {
            var monoBehaviours = FindObjectsOfType<MonoBehaviour>(includeInactive: true);
            foreach (var mb in monoBehaviours)
            {
                Container.GlobalInstance.Inject(mb);
            }
        }
    }
}
using Services.DataManagement;
using Services.Factories.Interfaces;
using Services.WindowsManagement.Windows;
using UnityEngine;

namespace Services.Factories
{
    public class WindowFactory : IWindowFactory
    {
        private readonly IStaticDataService _staticData;
        private readonly IInstantiator _instantiator;

        public WindowFactory(IStaticDataService staticData, IInstantiator instantiator)
        {
            _staticData = staticData;
            _instantiator = instantiator;
        }

        public WindowBase CreateWindow(WindowType windowType, Transform parent)
        {
            WindowBase prefab = _staticData.GetWindow(windowType);
            var window = _instantiator.InstantiatePrefabForComponent<WindowBase>(prefab, parent);
            
            return window;
        }
    }
}
using System;
using System.Collections.Generic;
using Plugins.RContainer;
using Services.Factories.Interfaces;
using Services.WindowsManagement.Windows;
using Tools.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Services.WindowsManagement
{
    public class WindowService : IWindowService
    {
        private readonly IWindowFactory _windowFactory;
        private readonly Stack<WindowBase> _windowStack = new();

        public WindowService(IWindowFactory windowFactory)
        {
            _windowFactory = windowFactory;
        }

        public void CloseWindow(Action onClosedAction = null)
        {
            if (_windowStack.IsEmpty()) return;

            _windowStack.Pop().Destroyself();
            onClosedAction?.Invoke();
        }

        public void CloseAllWindows(Action onClosedAction = null)
        {
            if (_windowStack.IsEmpty()) return;

            while (!_windowStack.IsEmpty()) CloseWindow();
            onClosedAction?.Invoke();
        }

        public WindowBase Open(WindowType windowType, Transform parent = null)
        {
            WindowBase instance = _windowFactory.CreateWindow(windowType, parent);
            Container.GlobalInstance.Inject(instance);
            _windowStack.Push(instance);
            
            instance.GetComponent<Canvas>().overrideSorting = true;
            instance.GetComponent<Canvas>().sortingOrder = _windowStack.Count;
            
            return _windowStack.Peek();
        }
    }
}
using System;
using Services.WindowsManagement.Windows;
using UnityEngine;

namespace Services.WindowsManagement
{
    public interface IWindowService
    {
        void CloseWindow(Action onClosedAction = null);
        WindowBase Open(WindowType windowType, Transform parent = null);
        void CloseAllWindows(Action onClosedAction = null);
    }
}
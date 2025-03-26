using System;
using Services.WindowsManagement.Windows;

namespace Configs
{
    [Serializable]
    public struct WindowData
    {
        public WindowBase Prefab;
        public WindowType TypeId;
    }
}
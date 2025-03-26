using Services.WindowsManagement.Windows;
using UnityEngine;

namespace Services.Factories.Interfaces
{
    public interface IWindowFactory
    {
        WindowBase CreateWindow(WindowType windowType, Transform parent);
    }
}
using Plugins.RContainer;
using Services.DataManagement;
using Services.DataManagement.ProgressData;
using UnityEngine;
using UnityEngine.UI;

namespace Services.WindowsManagement.Windows
{
    public abstract class WindowBase : MonoBehaviour
    {
        public Button CloseButton;

        private void OnEnable() =>
            OnEnableAction();

        private void OnDisable() =>
            OnDisableAction();

        private void Start() =>
            OnStartAction();

        public void Destroyself() =>
            Destroy(gameObject);

        protected virtual void OnEnableAction()
        {
            CloseButton.onClick.AddListener(CloseWindow);
        }

        protected virtual void OnDisableAction()
        {
            CloseButton.onClick.RemoveListener(CloseWindow);
        }

        protected virtual void OnStartAction()
        {
        }

        protected virtual void CloseWindow()
        {
            
        }
    }
}
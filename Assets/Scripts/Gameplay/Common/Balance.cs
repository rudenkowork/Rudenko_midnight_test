using Plugins.RContainer;
using Services.DataManagement.ProgressData;
using Services.EventBus;
using Services.WindowsManagement;
using Services.WindowsManagement.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Gameplay.Common
{
    public class Balance : MonoBehaviour
    {
        public TextMeshProUGUI Amount;

        private Button _shopButton;
        private IUserData _userData;
        private IWindowService _windowService;

        [Inject]
        private void Construct(IUserData userData, IWindowService windowService)
        {
            _windowService = windowService;
            _userData = userData;
        }
        
        private void OnEnable()
        {
            Container.GlobalInstance.Inject(this);
            _shopButton = GetComponent<Button>();
            _shopButton.onClick.AddListener(OpenShop);
            
            GameplayEventBus.Instance.OnBalanceChanged += SetBalance;
        }

        private void OnDisable()
        {
            _shopButton.onClick.RemoveListener(OpenShop);
            
            GameplayEventBus.Instance.OnBalanceChanged -= SetBalance;
        }

        private void Start() => 
            SetBalance();

        private void SetBalance() => 
            Amount.text = _userData.UserData.Wallet.Balance.ToString();

        private void OpenShop()
        {
            _windowService.Open(WindowType.SHOP);
        }
    }
}
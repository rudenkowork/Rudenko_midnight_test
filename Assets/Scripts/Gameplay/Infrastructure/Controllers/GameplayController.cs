using System;
using System.Collections.Generic;
using System.Linq;
using Gameplay.Food.Services;
using Gameplay.Infrastructure.Camera;
using Plugins.RContainer;
using Services.EventBus;
using Services.WindowsManagement;
using Services.WindowsManagement.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Infrastructure.Controllers
{
    public class GameplayController : MonoBehaviour
    {
        public Button InventoryButton;
        public Button NextBroadcasterButton;
        public Button PreviousBroadcasterButton;

        [Header("Orders")]
        public Button OrdersButton;
        public TextMeshProUGUI OrdersAmount;

        [Header("Sprites")]
        public Sprite HaveOrdersSprite;
        public Sprite HaveNoOrdersSprite;

        public List<BroadcasterData> Broadcasters;

        private IWindowService _windowService;
        private Dictionary<BroadcastType, Broadcaster> _broadcasters = new();
        private Broadcaster _currentBroadcast;
        private int _currentBroadcasterIndex = 1;
        private IBroadcastService _broadcastService;
        private IOrdersService _ordersService;

        [Inject]
        private void Construct(IWindowService windowService, IBroadcastService broadcastService,
            IOrdersService ordersService)
        {
            _ordersService = ordersService;
            _windowService = windowService;
            _broadcastService = broadcastService;
        }

        private void OnEnable()
        {
            OrdersButton.onClick.AddListener(OpenOrders);
            InventoryButton.onClick.AddListener(OpenInventory);
            NextBroadcasterButton.onClick.AddListener(NextBroadcaster);
            PreviousBroadcasterButton.onClick.AddListener(PreviousBroadcaster);

            GameplayEventBus.Instance.OnOrderPlaced += CheckOrders;
            GameplayEventBus.Instance.OnOrderDone += CheckOrders;
            GameplayEventBus.Instance.OnOrderEated += PlaceOrder;
        }

        private void OnDisable()
        {
            OrdersButton.onClick.RemoveListener(OpenOrders);
            InventoryButton.onClick.RemoveListener(OpenInventory);
            NextBroadcasterButton.onClick.RemoveListener(NextBroadcaster);
            PreviousBroadcasterButton.onClick.RemoveListener(PreviousBroadcaster);

            GameplayEventBus.Instance.OnOrderPlaced -= CheckOrders;
            GameplayEventBus.Instance.OnOrderDone -= CheckOrders;
            GameplayEventBus.Instance.OnOrderEated -= PlaceOrder;
        }

        private void Start()
        {
            _broadcasters = Broadcasters
                .ToDictionary(broadcast => broadcast.TypeId,
                    broadcast => broadcast.Broadcaster
                );

            HandleBroadcasters(BroadcastType.KITCHEN);

            PlaceOrder();
        }

        private void HandleBroadcasters(BroadcastType broadcastType)
        {
            if (_currentBroadcast != null)
            {
                _currentBroadcast.Disable();
            }

            _currentBroadcast = _broadcasters[broadcastType];
            _broadcastService.BroadcastType = broadcastType;
            _currentBroadcast.Enable();
        }

        private void CheckOrders()
        {
            if (_ordersService.Orders.Count > 0)
            {
                OrdersAmount.gameObject.SetActive(true);
                OrdersAmount.text = _ordersService.Orders.Count.ToString();
                OrdersButton.image.sprite = HaveOrdersSprite;
            }
            else
            {
                OrdersAmount.gameObject.SetActive(false);
                OrdersButton.image.sprite = HaveNoOrdersSprite;
            }
        }

        private void NextBroadcaster()
        {
            if (_currentBroadcasterIndex < Enum.GetValues(typeof(BroadcastType)).Length - 1)
            {
                _currentBroadcasterIndex++;
            }
            else
            {
                _currentBroadcasterIndex = 1;
            }

            HandleBroadcasters((BroadcastType)_currentBroadcasterIndex);
        }

        private void PreviousBroadcaster()
        {
            if (_currentBroadcasterIndex > 1)
            {
                _currentBroadcasterIndex--;
            }
            else
            {
                _currentBroadcasterIndex = 7;
            }

            HandleBroadcasters((BroadcastType)_currentBroadcasterIndex);
        }

        private void OpenOrders()
        {
            _windowService.Open(WindowType.ORDERS);
        }

        private void OpenInventory()
        {
            _windowService.Open(WindowType.INVENTORY);
        }

        private void PlaceOrder()
        {
            _ordersService.PlaceOrder();
        }
    }
}
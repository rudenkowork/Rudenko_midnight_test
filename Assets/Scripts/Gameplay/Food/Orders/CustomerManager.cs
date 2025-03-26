using System;
using Gameplay.Common;
using Services.EventBus;
using UnityEngine;

namespace Gameplay.Food.Orders
{
    public class CustomerManager : MonoBehaviour
    {
        public Transform PlatePosition;

        private void OnEnable()
        {
            GameplayEventBus.Instance.OnPlateReady += PastePlate;
        }

        private void OnDisable()
        {
            GameplayEventBus.Instance.OnPlateReady -= PastePlate;
        }

        private void PastePlate(Plate plate)
        {
            plate.transform.parent = PlatePosition;
            plate.transform.localPosition = Vector3.zero;

            plate.StartEating();
        }
    }
}
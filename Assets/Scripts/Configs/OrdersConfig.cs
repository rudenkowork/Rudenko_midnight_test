using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/Orders", fileName = "Orders")]
    public class OrdersConfig : ScriptableObject
    {
        public List<OrderData> Orders;
    }
}
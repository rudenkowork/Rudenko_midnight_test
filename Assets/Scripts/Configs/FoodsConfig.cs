using System.Collections.Generic;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "Foods", menuName = "Configs/Foods")]
    public class FoodsConfig : ScriptableObject
    {
        public List<FoodData> Foods;
    }
}
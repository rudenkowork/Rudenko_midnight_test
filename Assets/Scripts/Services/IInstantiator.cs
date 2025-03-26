using UnityEngine;

namespace Services
{
    public interface IInstantiator
    {
        TInstance InstantiatePrefabForComponent<TInstance>(TInstance prefab, Transform parent = null) where TInstance : MonoBehaviour;
    }
}
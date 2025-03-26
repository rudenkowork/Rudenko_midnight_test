using UnityEngine;

namespace Services.AssetManagement
{
    public interface IAssetProvider
    {
        TResource LoadResource<TResource>(string path) where TResource : Object;
    }
}
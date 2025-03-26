using UnityEngine;

namespace Services.AssetManagement
{
    public class AssetProvider : IAssetProvider
    {
        public TResource LoadResource<TResource>(string path) where TResource : Object =>
            Resources.Load<TResource>(path);
    }
}
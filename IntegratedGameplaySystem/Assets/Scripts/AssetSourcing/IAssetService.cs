using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// In the future, you could make this use:
    /// "addressable" or "asset bundle" or something else.
    /// </summary>
    public interface IAssetService
    {
        T GetAssetByType<T>() where T : UnityEngine.Object;
        List<T> GetAllAssetsOfType<T>() where T : UnityEngine.Object;
    }
}

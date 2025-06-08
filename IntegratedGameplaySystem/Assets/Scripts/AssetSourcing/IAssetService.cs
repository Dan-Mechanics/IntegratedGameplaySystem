using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// In the future, you could make this use:
    /// "addressable" or "asset bundle" or something else.
    /// I don't know what those terms mean yet. ( Something to research )
    /// </summary>
    public interface IAssetService
    {
        T GetAssetByType<T>() where T : UnityEngine.Object;
        List<T> GetAllAssetsOfType<T>() where T : UnityEngine.Object;
    }
}

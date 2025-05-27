using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// I think this is how you implement it ??
    /// </summary>
    [System.Serializable]
    public class AssetScratchpad : IAssetService, IStartable
    {
        public List<Object> assets;
        private readonly Dictionary<string, Object> scratchpad = new();

        public void Start()
        {
            assets.ForEach(x => scratchpad.Add(x.name, x));
        }

        public T FindAsset<T>(string name) where T : Object
        {
            if (!scratchpad.ContainsKey(name))
            {
                Debug.LogError($"Cant find it! Please fix the naming! {name}");
                return null;
            }

            return scratchpad[name] as T;
        }
    }

    public interface IAssetService 
    {
        T FindAsset<T>(string name) where T : Object;
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(AssetScratchpad), fileName = "New " + nameof(AssetScratchpad))]
    public class AssetScratchpad : ScriptableObject, IAssetService, IStartable
    {
        [SerializeField] private List<Object> assets = default;
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
}
using System.Collections.Generic;
using UnityEngine;
using System;

namespace IntegratedGameplaySystem
{
    [Serializable]
    public class InspectorAssets : IAssetService
    {
        [SerializeField] private List<UnityEngine.Object> assets = default;

        public T GetAssetByType<T>() where T : UnityEngine.Object
        {
            return assets.Find(x => x.GetType() == typeof(T)) as T;
        }

        public List<T> GetAllAssetsOfType<T>() where T : UnityEngine.Object
        {
            List<T> foundOfType = new List<T>();
            assets.FindAll(x => x.GetType() == typeof(T)).ForEach(x => foundOfType.Add(x as T));

            return foundOfType;
        }
    }
}
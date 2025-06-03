using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// This way, there is never confusion which assets do what.
    /// and because we use interface here, we can add adressables later.
    /// 
    /// Rename to insepctor assets, collection called assets.
    /// 
    /// 
    /// Code review: everything is pretty good but i dont wanna
    /// make a 
    /// 
    /// AssetBudnle term drop.
    /// </summary>
    [System.Serializable]
    public class InspectorAssets : IAssetService
    {
        /// <summary>
        /// Asin the collection of assets.
        /// </summary>
        [SerializeField] private List<Object> collection = default;

        /*public GameObject GetByAgreedName(string name)
        {
            return collection.Find(x => x.name == name) as GameObject;
        }

        public T GetByAgreedName<T>(string name) where T : Object
        {
            return collection.Find(x => x.name == name) as T;
        }*/

        public T GetAssetByType<T>() where T : Object
        {
            return collection.Find(x => x.GetType() == typeof(T)) as T;
        }

        public List<T> GetAssetsByType<T>() where T : Object
        {
            List<T> list = new List<T>();
            collection.FindAll(x => x.GetType() == typeof(T)).ForEach(x => list.Add(x as T));

            return list;
        }
    }
}
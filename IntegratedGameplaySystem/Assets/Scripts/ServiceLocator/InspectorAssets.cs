using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// This way, there is never confusion which assets do what.
    /// and because we use interface here, we can add adressables later.
    /// 
    /// this is a little strange? use resourc.elaod
    /// </summary>
    [System.Serializable]
    public class InspectorAssets : IAssetService
    {
        [SerializeField] private List<Object> assets = default;

        /*public GameObject GetByAgreedName(string name)
        {
            return collection.Find(x => x.name == name) as GameObject;
        }

        public T GetByAgreedName<T>(string name) where T : Object
        {
            return collection.Find(x => x.name == name) as T;
        }*/

        public T GetByType<T>() where T : Object
        {
            return assets.Find(x => x.GetType() == typeof(T)) as T;
        }

        public List<T> GetCollectionType<T>() where T : Object
        {
            List<T> result = new List<T>();
            assets.FindAll(x => x.GetType() == typeof(T)).ForEach(x => result.Add(x as T));

            return result;
        }
    }
}
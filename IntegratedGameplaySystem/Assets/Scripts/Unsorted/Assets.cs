using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// This way, there is never confusion which assets do what.
    /// and because we use interface here, we can add adressables later.
    /// </summary>
    [System.Serializable]
    public class Assets : IAssetService
    {
        [SerializeField] private List<Object> collection = default;

        public T GetByAgreedName<T>(string name) where T : Object
        {
            return collection.Find(x => x.name == name) as T;
        }

        public T GetByType<T>() where T : Object
        {
            return collection.Find(x => x.GetType() == typeof(T)) as T;
        }
    }
}
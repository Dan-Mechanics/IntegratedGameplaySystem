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

        /*public List<PlantSpeciesProfile> GetPlants() 
        {
            return collection.FindAll(x => x.GetType() == typeof(PlantSpeciesProfile)) as List<PlantSpeciesProfile>;
        }*/

        public GameObject GetByAgreedName(string name) 
        {
            return collection.Find(x => x.name == name) as GameObject;
        }

        public T GetByAgreedName<T>(string name) where T : Object
        {
            return collection.Find(x => x.name == name) as T;
        }

        public T GetByType<T>() where T : Object
        {
            return collection.Find(x => x.GetType() == typeof(T)) as T;
        }

        /// <summary>
        /// Idk how to do these better sry.
        /// </summary>
        public List<T> GetCollectionType<T>() where T : Object
        {
            List<Object> filteredCollection = collection.FindAll(x => x.GetType() == typeof(T));
            List<T> result = new List<T>();

            filteredCollection.ForEach(x => result.Add(x as T));

            return result;
        }
    }
}
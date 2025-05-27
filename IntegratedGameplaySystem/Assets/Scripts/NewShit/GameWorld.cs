using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class GameWorld : IWorldService
    {
        private readonly Dictionary<GameObject, List<object>> world = new();

        public List<object> GetObjects(GameObject go) 
        {
            if (!world.ContainsKey(go))
                return null;

            return world[go];
        }

        public T GetComponent<T>(GameObject go) 
        {
            if (GetObjects(go) is not List<object> objects)
                return default;

            foreach (object obj in objects)
            {
                if (obj is T t)
                    return t;
            }

            return default;
        }
    }

    public interface IWorldService 
    {
        T GetComponent<T>(GameObject go);
    }
}
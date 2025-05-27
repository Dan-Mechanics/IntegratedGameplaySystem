using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class GameWorld : IWorldService
    {
        private readonly Dictionary<GameObject, List<object>> world = new();

        public T GetComponent<T>(GameObject go) 
        {
            if (!world.ContainsKey(go))
                return default;

            foreach (object obj in world[go])
            {
                if (obj is T t)
                    return t;
            }

            return default;
        }
    }
}
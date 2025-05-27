using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Only use this for shit that needs it basically.
    /// 
    /// keep to: YAGNI here.
    /// </summary>
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

        public void Add(GameObject go) 
        {
            if (world.ContainsKey(go))
                return;

            world.Add(go, new List<object>());
        }

        public void Remove(GameObject go) { world.Remove(go); }
    }
}
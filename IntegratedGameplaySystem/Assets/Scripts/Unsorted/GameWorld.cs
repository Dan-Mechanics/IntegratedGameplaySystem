using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Only use this for shit that needs it basically.
    /// 
    /// keep to: YAGNI here.
    /// 
    /// IDEA: make the game world and gamecotnext one entity that also handles shit being destroyed right?
    /// 
    /// 
    /// In the future i can make the object object LISt but for now this is good.
    /// </summary>
    public class GameWorld : IWorldService
    {
        private readonly Dictionary<GameObject, object> world = new();

        public T GetComponent<T>(GameObject go) 
        {
            if (!world.ContainsKey(go))
                return default;

            if (world[go] is T t)
                return t;

            return default;
        }

        public void Add(GameObject go, object obj) 
        {
            if (world.ContainsKey(go))
                return;

            world.Add(go, obj);
        }

        public void Remove(GameObject go) => world.Remove(go);
    }
}
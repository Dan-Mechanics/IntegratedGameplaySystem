using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class GameWorld : IWorldService
    {
        private readonly Dictionary<Transform, object> world = new();

        public T GetComponent<T>(Transform transform)
        {
            if (!world.ContainsKey(transform))
                return default;

            if (world[transform] is T t)
                return t;

            return default;
        }

        public void Add(Transform transform, object obj) 
        {
            if (world.ContainsKey(transform))
                return;

            world.Add(transform, obj);
        }

        public void Remove(Transform transform) => world.Remove(transform);

        public void Reset() => world.Clear();
    }
}
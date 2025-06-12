using UnityEngine;

namespace IntegratedGameplaySystem
{
    public interface IWorldService
    {
        T GetComponent<T>(Transform transform);
        void Add(Transform transform, object obj);
        void Remove(Transform transform);
        void Reset();
    }
}
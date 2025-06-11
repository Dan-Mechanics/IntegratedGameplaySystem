using UnityEngine;

namespace IntegratedGameplaySystem
{
    public interface IWorldService
    {
        T GetComponent<T>(Transform trans);
        void Add(Transform go, object obj);
        void Remove(Transform go);
        void Reset();
    }
}
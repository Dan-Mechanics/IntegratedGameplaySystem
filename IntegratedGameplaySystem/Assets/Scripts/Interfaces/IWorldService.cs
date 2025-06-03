using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    public interface IWorldService
    {
        T GetComponent<T>(GameObject go);
        void Add(GameObject go, object obj);
        void Remove(GameObject go);
        void Reset();
    }

    public interface IScoreService
    {
        float GetScore();
    }
}
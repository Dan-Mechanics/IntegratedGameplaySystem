using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Make this work with transform only !!
    /// </summary>
    public interface IWorldService
    {
        T GetComponent<T>(GameObject go);
        T GetComponent<T>(Transform trans);
        void Add(GameObject go, object obj);
        void Remove(GameObject go);
        void Reset();
        //bool Contains(Transform transform);
    }

    public interface IScoreService
    {
        float GetScore();
    }
}
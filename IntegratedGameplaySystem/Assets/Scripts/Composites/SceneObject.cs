using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Because we favor compositon over inhertience.
    /// </summary>
    public class SceneObject
    {
        public GameObject gameObject;
        public Transform transform;

        public SceneObject(GameObject prefab)
        {
            gameObject = Utils.SpawnPrefab(prefab);
            transform = gameObject.transform;
        }
    }
}
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

        public SceneObject(string prefabName)
        {
            gameObject = Utils.SpawnPrefab(ServiceLocator<IAssetService>.Locate().GetByAgreedName(prefabName));
            transform = gameObject.transform;
        }
    }
}
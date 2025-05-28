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
        public Transform trans;

        public SceneObject(string prefabName)
        {
            gameObject = ServiceLocator<IAssetService>.Locate().FindAsset<GameObject>(prefabName);
            trans = gameObject.transform;
        }
    }
}
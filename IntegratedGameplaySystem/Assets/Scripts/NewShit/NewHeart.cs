using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// because we want to favor compositon over inhertience.
    /// </summary>
    public class NewHeart
    {
        public GameObject go;
        public Transform trans;

        public SceneObject(string name)
        {
            // or something.
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/{name}");

            go = Object.Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
            trans = go.transform;
        }
    }
}
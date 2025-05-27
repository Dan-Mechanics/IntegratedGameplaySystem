using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// because we want to favor compositon over inhertience.
    /// </summary>
    public class SceneObject
    {
        public GameObject go;
        public Transform trans;

        public SceneObject(string name)
        {
            name = name.ToLower();
            
            // or something.
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/{name}");

            // add debug here.

            go = Object.Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
            trans = go.transform;
        }
    }
}
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Because we favor compositon over inhertience.
    /// </summary>
    public class SceneObject
    {
        public readonly GameObject gameObject;
        public readonly Transform transform;

        public SceneObject(GameObject prefab)
        {
            gameObject = Utils.SpawnPrefab(prefab);
            transform = gameObject.transform;
        }
    }
}
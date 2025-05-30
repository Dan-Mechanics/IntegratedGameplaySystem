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

        public SceneObject(string prefabName)
        {
            gameObject = Utils.SpawnPrefab(ServiceLocator<IAssetService>.Locate().GetByAgreedName(prefabName));
            transform = gameObject.transform;
        }

        public SceneObject(GameObject prefab)
        {
            gameObject = Utils.SpawnPrefab(prefab);
            transform = gameObject.transform;
        }
    }
}
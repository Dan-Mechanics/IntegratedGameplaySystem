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
            gameObject.name = prefabName;

            transform = gameObject.transform;
        }
    }
}
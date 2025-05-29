using UnityEngine;

namespace IntegratedGameplaySystem
{
    [System.Serializable]
    public class SmoothAssets : IAssetService
    {
        public GameObject GetPlayer() => player as GameObject;
        public RaycastData GetInteractionRaycast() => interactionRaycast as RaycastData;
        public MovementSettings GetMovementSettings() => movementSettings as MovementSettings;
        public GroundedConfiguration GetGroundedConfig() => groundedConfiguration as GroundedConfiguration;
        public BindingsConfig GetBindingsConfig() => bindingsConfig as BindingsConfig;

        [SerializeField] private Object player = default;
        [SerializeField] private Object interactionRaycast = default;
        [SerializeField] private Object movementSettings = default;
        [SerializeField] private Object groundedConfiguration = default;
        [SerializeField] private Object bindingsConfig = default;
    }
}
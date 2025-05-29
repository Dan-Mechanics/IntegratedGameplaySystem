using UnityEngine;
using System;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// This way, there is never confusion which assets do what.
    /// and because we use interface here, we can add adressables later.
    /// </summary>
    [Serializable]
    public class Assets : IAssetService
    {
        public GameObject GetPlayer() => player;
        public RaycastSettings GetInteractionRaycast() => interactionRaycast;
        public PlayerSettings GetMovementSettings() => movementSettings;
      // public GroundedConfiguration GetGroundedConfig() => groundedConfiguration;
      // public CameraSettingsFPS GetSettingsFPS() => cameraSettingsFPS;
        public BindingsConfig GetBindingsConfig() => bindingsConfig;

        [SerializeField] private GameObject player = default;
        [SerializeField] private RaycastSettings interactionRaycast = default;
        [SerializeField] private PlayerSettings movementSettings = default;
        // [SerializeField] private GroundedConfiguration groundedConfiguration = default;
        // [SerializeField] private CameraSettingsFPS cameraSettingsFPS = default;
        [SerializeField] private BindingsConfig bindingsConfig = default;
    }
}
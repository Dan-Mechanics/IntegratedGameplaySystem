using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// I want the context classes to load shit from memory basically.
    /// </summary>
    public class PlayerContext : IStartable, IUpdatable, IFixedUpdatable, ILateFixedUpdatable
    {
        private Transform eyes;
        private readonly PlayerInput playerInput = new PlayerInput();
        private ForcesMovement movement;
        private MouseMovement mouseMovement;
        private CameraHandler cameraHandler;

        public void Start()
        {
            IAssetService assets = ServiceLocator<IAssetService>.Locate();
            Transform player = Utils.SpawnPrefab(assets.GetPlayer()).transform;
            CameraSettingsFPS fpsSettings = assets.GetSettingsFPS();

            eyes = new GameObject("eyes").transform;
            eyes.SetParent(player);
            eyes.localPosition = Vector3.up * fpsSettings.eyesHeight;

            movement = new ForcesMovement(player, eyes, assets.GetGroundedConfig(), assets.GetMovementSettings());
            mouseMovement = new MouseMovement(eyes, player, fpsSettings.sens);
            cameraHandler = new CameraHandler(Camera.main.transform);
        }

        public void Update()
        {
            mouseMovement.Update(playerInput.GetMouseInput());

            cameraHandler.UpdateRot(eyes.rotation);
            cameraHandler.Update();
        }

        public void FixedUpdate()
        {
            movement.DoMovement(playerInput.Vertical(), playerInput.Horizontal());
        }

        public void LateFixedUpdate()
        {
            cameraHandler.SetTick(movement.GetTick());
        }
    }
}
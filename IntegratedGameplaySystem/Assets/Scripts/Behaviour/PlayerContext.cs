using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// I want the context classes to load shit from memory basically.
    /// 
    /// Renmae to player or something. playerbehavuour.
    /// </summary>
    public class PlayerContext : IStartable, IUpdatable, IFixedUpdatable, ILateFixedUpdatable
    {
        public const string PLAYER_PREFAB_NAME = "player";
        
        private Transform eyes;
        private readonly PlayerInput playerInput = new PlayerInput();
        private ForcesMovement movement;
        private MouseMovement mouseMovement;
        private CameraHandler cameraHandler;

        public void Start()
        {
            IAssetService assets = ServiceLocator<IAssetService>.Locate();

            Transform player = Utils.SpawnPrefab(assets.GetByAgreedName<GameObject>(PLAYER_PREFAB_NAME)).transform;
            PlayerSettings settings = assets.GetByType<PlayerSettings>();

            eyes = new GameObject("eyes").transform;
            eyes.SetParent(player);
            eyes.localPosition = Vector3.up * settings.eyesHeight;

            movement = new ForcesMovement(player, eyes, settings);
            mouseMovement = new MouseMovement(eyes, player, settings.sens);
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
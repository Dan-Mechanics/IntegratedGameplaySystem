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
        private readonly IPlayerInput playerInput = new KeyboardSource();
        
        private Transform eyes;
        private ForcesMovement movement;
        private MouseMovement mouseMovement;
        private CameraHandler cameraHandler;

        public void Start()
        {
            IAssetService assets = ServiceLocator<IAssetService>.Locate();

            SceneObject player = new SceneObject(PLAYER_PREFAB_NAME);
            PlayerSettings settings = assets.GetByType<PlayerSettings>();

            eyes = new GameObject("eyes").transform;
            eyes.SetParent(player.transform);
            eyes.localPosition = Vector3.up * settings.eyesHeight;

            movement = new ForcesMovement(player.transform, eyes, settings);
            mouseMovement = new MouseMovement(eyes, player.transform, settings.sens);
            cameraHandler = new CameraHandler(Camera.main.transform);
        }

        public void Update()
        {
            mouseMovement.Update(playerInput.GetLookingInput());

            cameraHandler.UpdateRot(eyes.rotation);
            cameraHandler.Update();
        }

        public void FixedUpdate()
        {
            movement.DoMovement(playerInput.GetVertical(), playerInput.GetHorizontal());
        }

        public void LateFixedUpdate()
        {
            cameraHandler.SetTick(movement.GetTick());
        }
    }
}
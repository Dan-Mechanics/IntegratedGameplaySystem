using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class Player : IStartable, IUpdatable, IFixedUpdatable, ILateFixedUpdatable
    {
        private readonly IPlayerInput playerInput;
        
        private Transform eyes;
        private ForcesMovement movement;
        private MouseMovement mouseMovement;
        private CameraExtrapolation cameraHandler;

        public Player(IPlayerInput playerInput)
        {
            this.playerInput = playerInput;
        }

        public void Start()
        {
            PlayerSettings settings = ServiceLocator<IAssetService>.Locate().GetAssetWithType<PlayerSettings>();
            Transform transform = Utils.SpawnPrefab(settings.prefab).transform;

            eyes = new GameObject("eyes").transform;
            eyes.SetParent(transform);
            eyes.localPosition = Vector3.up * settings.eyesHeight;

            movement = new ForcesMovement(transform, eyes, settings);
            mouseMovement = new MouseMovement(eyes, transform, settings.sens);
            cameraHandler = new CameraExtrapolation(Camera.main.transform);
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
            cameraHandler.SetSnapshot(movement.GetSnapshot());
        }
    }
}
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class Player : IStartable, IUpdatable, IFixedUpdatable, ILateFixedUpdatable
    {
        private readonly IPlayerInput playerInput;
        
        private Transform eyes;
        private ForcesMovement movement;
        private MouseMovement mouseMovement;
        private CameraHandler cameraHandler;

        public Player(IPlayerInput playerInput)
        {
            this.playerInput = playerInput;
        }

        public void Start()
        {
            PlayerSettings settings = ServiceLocator<IAssetService>.Locate().GetByType<PlayerSettings>();
            SceneObject player = new SceneObject(settings.prefab);

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
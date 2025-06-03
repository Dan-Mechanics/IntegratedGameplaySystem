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
            PlayerSettings settings = ServiceLocator<IAssetService>.Locate().GetAssetByType<PlayerSettings>();
            SceneObject sceneObject = new SceneObject(settings.prefab);

            eyes = new GameObject("eyes").transform;
            eyes.SetParent(sceneObject.transform);
            eyes.localPosition = Vector3.up * settings.eyesHeight;

            movement = new ForcesMovement(sceneObject.transform, eyes, settings);
            mouseMovement = new MouseMovement(eyes, sceneObject.transform, settings.sens);
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
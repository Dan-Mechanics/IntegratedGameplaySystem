using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class FirstPersonPlayer : IUpdatable, IFixedUpdatable, ILateFixedUpdatable
    {
        public ForcesMovement Movement { get; private set; }
        private readonly IPlayerInput playerInput;
        
        private readonly Transform eyes;
        private readonly MouseMovement mouseMovement;
        private readonly CameraExtrapolation cameraHandler;

        public FirstPersonPlayer(IPlayerInput playerInput)
        {
            this.playerInput = playerInput;

            PlayerSettings settings = ServiceLocator<IAssetService>.Locate().GetAssetByType<PlayerSettings>();
            Transform transform = Utils.SpawnPrefab(settings.prefab).transform;

            eyes = new GameObject("eyes").transform;
            eyes.SetParent(transform);
            eyes.localPosition = Vector3.up * settings.eyesHeight;

            Movement = new ForcesMovement(transform, eyes, settings);
            mouseMovement = new MouseMovement(eyes, transform, settings.sens);
            cameraHandler = new CameraExtrapolation(Camera.main.transform);
        }

        /*public void Start()
        {
            PlayerSettings settings = ServiceLocator<IAssetService>.Locate().GetAssetByType<PlayerSettings>();
            Transform transform = Utils.SpawnPrefab(settings.prefab).transform;

            eyes = new GameObject("eyes").transform;
            eyes.SetParent(transform);
            eyes.localPosition = Vector3.up * settings.eyesHeight;

            Movement = new ForcesMovement(transform, eyes, settings);
            mouseMovement = new MouseMovement(eyes, transform, settings.sens);
            cameraHandler = new CameraExtrapolation(Camera.main.transform);
        }*/

        public void Update()
        {
            mouseMovement.Update(playerInput.GetLookingInput());

            cameraHandler.UpdateRot(eyes.rotation);
            cameraHandler.Update();
        }

        public void FixedUpdate()
        {
            Movement.DoMovement(playerInput.GetVertical(), playerInput.GetHorizontal());
        }

        public void LateFixedUpdate()
        {
            cameraHandler.SetSnapshot(Movement.GetSnapshot());
        }
    }
}
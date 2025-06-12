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

        private ExtrapolationSnapshot snapshot;

        public FirstPersonPlayer(IPlayerInput playerInput, Sensitivity sensitivity)
        {
            this.playerInput = playerInput;

            PlayerSettings settings = ServiceLocator<IAssetService>.Locate().GetAssetByType<PlayerSettings>();
            Transform transform = Utils.SpawnPrefab(settings.prefab).transform;

            eyes = new GameObject("eyes").transform;
            eyes.SetParent(transform);
            eyes.localPosition = Vector3.up * settings.eyesHeight;

            Movement = new ForcesMovement(transform, eyes, settings);
            mouseMovement = new MouseMovement(eyes, transform, sensitivity);
            cameraHandler = new CameraExtrapolation(Camera.main.transform);
        }

        public void Update()
        {
            mouseMovement.Update(playerInput.GetLookingInput());

            cameraHandler.UpdateCameraRotation(eyes.rotation);
            cameraHandler.Update();
        }

        public void FixedUpdate()
        {
            Movement.DoMovement(playerInput.GetVertical(), playerInput.GetHorizontal());
        }

        public void LateFixedUpdate()
        {
            Movement.GetClampedSnapshot(out Vector3 eyes, out Vector3 vel, out float time);
            snapshot.Set(eyes, vel, time);
            cameraHandler.SetSnapshot(snapshot);
        }
    }
}
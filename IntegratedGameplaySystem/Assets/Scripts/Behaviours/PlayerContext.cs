using UnityEngine;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = nameof(BaseBehaviour) + "/" + nameof(PlayerContext), fileName = "New " + nameof(PlayerContext))]
    public class PlayerContext : BaseBehaviour
    {
        public ForcesMovement.Settings settings;
        public ForcesMovement.GroundedConfiguration grounded;
        public float eyeHeight;
        public Wallet wallet;

        private Rigidbody rb;
        private Transform eyes;
        private PlayerInput playerInput;
        private CameraHandler cameraHandler;
        private MouseMovement mouseMovement;
        private ForcesMovement movement;

        public override void Start()
        {
            base.Start();
            rb = transform.GetComponent<Rigidbody>();

            eyes = new GameObject("eyes").transform;
            eyes.SetParent(transform);
            eyes.localPosition = Vector3.up * eyeHeight;
            playerInput = new PlayerInput();

            ForcesMovement.References player = new ForcesMovement.References(rb, eyes, transform);
            movement = new ForcesMovement(grounded, settings, player);
            mouseMovement = new MouseMovement(eyes, transform);
            cameraHandler = new CameraHandler(Camera.main.transform);
        }

        public override void Update()
        {
            base.Update();

            mouseMovement.Update(playerInput.GetMouseInput());

            cameraHandler.UpdateRot(eyes.rotation);
            cameraHandler.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            movement.DoMovement(playerInput.Vertical(), playerInput.Horizontal());
        }

        public override void LateFixedUpdate()
        {
            base.LateFixedUpdate();
            cameraHandler.SetTick(movement.GetTick());
        }

        public override void Disable()
        {
            base.Disable();
            playerInput.Dispose();
        }
    }
}
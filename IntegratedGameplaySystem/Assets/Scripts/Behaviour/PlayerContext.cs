using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// I want the context classes to load shit from memory basically.
    /// </summary>
    public class PlayerContext : IStartable, IUpdatable
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

        private readonly SceneObject obj;

        public PlayerContext()
        {
            obj = new SceneObject("player");
        }

        public void Start()
        {
            rb = obj.trans.GetComponent<Rigidbody>();

            eyes = new GameObject("eyes").transform;
            eyes.SetParent(obj.trans);
            eyes.localPosition = Vector3.up * eyeHeight;

            playerInput = new PlayerInput();

            ForcesMovement.References player = new ForcesMovement.References(rb, eyes, obj.trans);
            movement = new ForcesMovement(grounded, settings, player);
            mouseMovement = new MouseMovement(eyes, obj.trans);
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
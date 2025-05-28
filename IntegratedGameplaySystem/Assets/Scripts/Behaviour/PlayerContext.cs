using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// I want the context classes to load shit from memory basically.
    /// </summary>
    public class PlayerContext : IStartable, IUpdatable, IFixedUpdatable, ILateFixedUpdatable
    {
        /// <summary>
        /// EXPORT TO SOME ASSET !!
        /// </summary>
        public const float EYES_HEIGHT = 0.2f;

        private Transform eyes;
        private readonly PlayerInput playerInput = new PlayerInput();
        private ForcesMovement movement;
        private MouseMovement mouseMovement;
        private CameraHandler cameraHandler;

        public void Start()
        {
            SceneObject player = new SceneObject("player");

            Debug.Log(player.trans);

            Rigidbody rb = player.trans.GetComponent<Rigidbody>();

            eyes = new GameObject("eyes").transform;
            eyes.SetParent(player.trans);
            eyes.localPosition = Vector3.up * EYES_HEIGHT;

            movement = new ForcesMovement(player.trans, eyes, rb);
            mouseMovement = new MouseMovement(eyes, player.trans);
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
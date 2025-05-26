using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// And this would then be where we have our like input handler classes and such. Component pattern perchange ??? !!
    /// 
    /// PlayerBehaviour??
    /// </summary>
    [CreateAssetMenu(menuName = nameof(BaseBehaviour) + "/" + nameof(PlayerContext), fileName = "New " + nameof(PlayerContext))]
    public class PlayerContext : BaseBehaviour
    {
        public ForcesMovement.Settings settings;
        public ForcesMovement.GroundedConfiguration grounded;
        public float eyeHeight;
        public InteractBehaviour interactBehaviour;
        public Wallet wallet;
        [Tooltip("Service locator to filesystem to load config.")]
        public List<InputHandler.Binding> bindings;

        private InputHandler inputHandler;
        private Rigidbody rb;
        private Transform eyes;
        private PlayerInput playerInput;
        private CameraHandler handler;
        private MouseMovement mouseMovement;
        private ForcesMovement movement;

        public override void Start()
        {
            base.Start();
            rb = transform.GetComponent<Rigidbody>();

            eyes = new GameObject("eyes").transform;
            eyes.SetParent(transform);
            eyes.localPosition = Vector3.up * eyeHeight;

            inputHandler = new InputHandler(bindings);
            playerInput = new PlayerInput(inputHandler);

            ForcesMovement.References references = new ForcesMovement.References(rb, eyes, transform);
            movement = new ForcesMovement(grounded, settings, references);
            mouseMovement = new MouseMovement(eyes, transform);
            handler = new CameraHandler(Camera.main.transform);

            interactBehaviour.SetInputHandler(inputHandler);
        }

        public override void Update()
        {
            base.Update();

            inputHandler.Update();
            mouseMovement.Update(playerInput.GetMouseInput());

            handler.UpdateRot(eyes.rotation);
            handler.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            movement.DoMovement(playerInput.Vertical(), playerInput.Horizontal());
        }

        public override void LateFixedUpdate()
        {
            base.LateFixedUpdate();
            handler.SetTick(movement.GetTick());
        }

        public override void Disable()
        {
            base.Disable();
            playerInput.Dispose();
        }
    }
}
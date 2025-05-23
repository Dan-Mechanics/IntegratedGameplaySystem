using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// And this would then be where we have our like input handler classes and such.
    /// </summary>
    [CreateAssetMenu(menuName = nameof(BaseBehaviour) + "/" + nameof(PlayerContext), fileName = "New " + nameof(PlayerContext))]
    public class PlayerContext : BaseBehaviour
    {
        public ForcesMovement.Settings settings;
        public ForcesMovement.GroundedConfiguration grounded;

        private Rigidbody rb;
        private Transform eyes;
        private readonly PlayerInput playerInput = new PlayerInput();
        private CameraHandler handler;
        private MouseMovement mouseMovement;
        private ForcesMovement movement;

        public override void Start()
        {
            base.Start();
            rb = trans.GetComponent<Rigidbody>();
            eyes = trans.GetChild(0);

            ForcesMovement.References references = new ForcesMovement.References(rb, eyes, trans);
            movement = new ForcesMovement(grounded, settings, references);
            mouseMovement = new MouseMovement(eyes, trans);
            handler = new CameraHandler(Camera.main.transform);
        }

        public override void Update()
        {
            base.Update();

            mouseMovement.Update(playerInput.GetMouseInput());
            handler.UpdateRot(eyes.rotation);
            handler.Update();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            handler.SetTick(movement.DoMovement(playerInput.GetMovementDirection()));
        }
    }
}
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// And this would then be where we have our like input handler classes and such.
    /// </summary>
    [CreateAssetMenu(menuName = nameof(BaseBehaviour) + "/" + nameof(PlayerContext), fileName = "New " + nameof(PlayerContext))]
    public class PlayerContext : BaseBehaviour
    {
        [Min(1f)] public float speed;
        public ForcesMovement.GroundedConfiguration config;
        public ForcesMovement.Settings settings;

        private Rigidbody rb;
        private Transform eyes;
        private PlayerInput playerInput;
        private CameraHandler handler;
        private MouseMovement mouseMovement;
        private ForcesMovement movement;

        public override void Start()
        {
            base.Start();
            rb = trans.GetComponent<Rigidbody>();
            eyes = trans.GetChild(0);

            ForcesMovement.References references = new ForcesMovement.References(rb, eyes, trans);
            movement = new ForcesMovement(config, settings, references);
            mouseMovement = new MouseMovement(eyes, trans);
            handler = new CameraHandler(Camera.main.transform);
        }

        public override void Update()
        {
            base.Update();

            handler.Update();
            mouseMovement.Update(playerInput.GetMouseInput());
            handler.UpdateRot(eyes.rotation);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            
            // handler.
        }

    }
}
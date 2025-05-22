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
        private Rigidbody rb;
        
        public override void Start()
        {
            base.Start();
            rb = trans.GetComponent<Rigidbody>();


        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            rb.velocity = Vector3.right * speed;
        }
    }
}
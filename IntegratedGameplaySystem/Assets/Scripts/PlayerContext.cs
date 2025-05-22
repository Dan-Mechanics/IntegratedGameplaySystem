using UnityEngine;

namespace IntegratedGameplaySystem
{
    [CreateAssetMenu(menuName = nameof(BaseBehaviour) + "/" + nameof(PlayerContext), fileName = "New " + nameof(PlayerContext))]
    public class PlayerContext : BaseBehaviour
    {
        [Min(1f)] public float speed;
        private Rigidbody rb;
        
        public override void Start()
        {
            base.Start();
            rb = Fetch<Rigidbody>();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            print(pos.ToString());
            rb.velocity = Vector3.right * speed;
        }
    }
}
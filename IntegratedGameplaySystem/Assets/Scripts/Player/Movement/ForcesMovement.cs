using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class ForcesMovement
    {
        private readonly PlayerSettings settings;
        private readonly Transform trans;
        private readonly Transform eyes;
        private readonly Rigidbody rb;

        private ISpeedSource speedSource;
        private bool isGrounded;

        public ForcesMovement(Transform trans, Transform eyes, PlayerSettings settings)
        {
            this.trans = trans;
            this.eyes = eyes;
            this.settings = settings;
            rb = trans.GetComponent<Rigidbody>();
            SetSpeedSource(settings);
        }

        public void SetSpeedSource(ISpeedSource speedSource) => this.speedSource = speedSource;

        public void DoMovement(float vert, float hori)
        {
            isGrounded = GetIsGrounded();

            float accel = isGrounded ? settings.movAccel : settings.movAccel * settings.accelMult;
            Vector3 mov = GetMovementDirection(vert, hori, trans);

            Vector3 flatVel = rb.velocity;
            flatVel.y = 0f;

            float mag = flatVel.magnitude;
            
            rb.AddForce(accel * Time.fixedDeltaTime * mov, ForceMode.VelocityChange);

            Vector3 counterMovement = accel * Time.fixedDeltaTime * settings.accelMult * -(flatVel.normalized - mov);
            if (mag >= 0f && counterMovement.magnitude > mag)
                counterMovement = -flatVel;

            rb.AddForce(counterMovement, ForceMode.VelocityChange);
        }

        public void GetClampedSnapshot(out Vector3 eyesPos, out Vector3 velocity, out float time)
        {
            velocity = Vector3.ClampMagnitude(rb.velocity, speedSource.GetSpeed());
            rb.velocity = velocity;
            eyesPos = eyes.position;
            time = Time.time;
        }

        private Vector3 GetMovementDirection(float vert, float hori, Transform trans) 
        {
            Vector3 mov = Vector3.zero;

            mov += trans.right * hori;
            mov += trans.forward * vert;
            mov.Normalize();

            return mov;
        }

        private bool GetIsGrounded()
        {
            RaycastHit[] hits = Physics.SphereCastAll(trans.position, settings.radius, Vector3.down, settings.reach, settings.mask, QueryTriggerInteraction.Ignore);

            foreach (RaycastHit hit in hits)
            {
                if (hit.distance == 0f)
                    continue;

                if (Vector3.Angle(Vector3.up, hit.normal) <= settings.maxAngle)
                    return true;
            }

            return false;
        }
    }
}
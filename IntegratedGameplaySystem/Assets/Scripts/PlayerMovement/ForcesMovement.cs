using UnityEngine;
using System;

namespace IntegratedGameplaySystem
{
    public class ForcesMovement
    {
        /// <summary>
        /// !Coupling !!
        /// Make different to camerhandler and give better name.
        /// </summary>
        private CameraHandler.Tick currentTick;
        private readonly GroundedConfiguration groundedConfig;
        private readonly Settings settings;
        private readonly References references;

        public ForcesMovement(GroundedConfiguration groundedConfig, Settings settings, References references)
        {
            this.groundedConfig = groundedConfig;
            this.settings = settings;
            this.references = references;
        }

        /// <summary>
        /// This code fucking sux. Fit it!
        /// </summary>
        public CameraHandler.Tick DoMovement(Vector3 input) 
        {
            bool isGrounded = GetIsGrounded(groundedConfig, references.trans.position);

            references.rb.velocity = Vector3.ClampMagnitude(references.rb.velocity, isGrounded ? settings.runSpeed : settings.flySpeed);

            currentTick.Set(references.eyes.position, references.rb.velocity, Time.time);

            float acceleration = isGrounded ? settings.movAccel : settings.movAccel * settings.airborneAccelMult;

            Vector3 movement = GetMovement(input, references);

            Vector3 velocity = references.rb.velocity;
            velocity.y = 0f;
            float mag = velocity.magnitude;

            if (mag < settings.walkSpeed)
            {
                references.rb.AddForce(Vector3.ClampMagnitude(acceleration * Time.fixedDeltaTime * movement, settings.walkSpeed - mag), ForceMode.VelocityChange);
            }
            else if (isGrounded)
            {
                references.rb.AddForce(Vector3.ClampMagnitude(acceleration * Time.fixedDeltaTime * -velocity.normalized, mag - settings.walkSpeed), ForceMode.VelocityChange);
            }

            Vector3 counterMovement = acceleration * Time.fixedDeltaTime * settings.airborneAccelMult * -(velocity.normalized - movement);
            if (mag != 0f && counterMovement.magnitude > mag)
                counterMovement = -velocity;

            references.rb.AddForce(counterMovement, ForceMode.VelocityChange);

            return currentTick;
        }

        private Vector3 GetMovement(Vector3 input, References references) 
        {
            Vector3 mov = Vector3.zero;

            mov += references.trans.right * input.x;
            mov += references.trans.forward * input.z;
            mov.Normalize();

            return mov;
        }

        private bool GetIsGrounded(GroundedConfiguration config, Vector3 playerPosition)
        {
            RaycastHit[] hits = Physics.SphereCastAll(playerPosition, config.groundColliderRadius, Vector3.down, config.groundColliderDownward, config.groundMask, QueryTriggerInteraction.Ignore);

            foreach (RaycastHit hit in hits)
            {
                if (hit.distance == 0f)
                    continue;

                if (Vector3.Angle(Vector3.up, hit.normal) <= config.maxGroundedAngle)
                    return true;
            }

            return false;
        }

        [Serializable]
        public class GroundedConfiguration 
        {
            public LayerMask groundMask;
            public float groundColliderRadius;
            public float groundColliderDownward;
            public float maxGroundedAngle;

            public GroundedConfiguration(LayerMask groundMask, float groundColliderRadius, float groundColliderDownward, float maxGroundedAngle)
            {
                this.groundMask = groundMask;
                this.groundColliderRadius = groundColliderRadius;
                this.groundColliderDownward = groundColliderDownward;
                this.maxGroundedAngle = maxGroundedAngle;
            }
        }

        [Serializable]
        public class References 
        {
            public Rigidbody rb;
            public Transform eyes;
            public Transform trans;

            public References(Rigidbody rb, Transform eyes, Transform trans)
            {
                this.rb = rb;
                this.eyes = eyes;
                this.trans = trans;
            }
        }

        /// <summary>
        /// Consider making these scriptable objects too.
        /// </summary>
        [Serializable]
        public class Settings 
        {
            public float walkSpeed;
            public float runSpeed;
            public float flySpeed;
            public float movAccel;
            public float airborneAccelMult;

            public Settings(float walkSpeed, float runSpeed, float flySpeed, float movAccel, float airborneAccelMult)
            {
                this.walkSpeed = walkSpeed;
                this.runSpeed = runSpeed;
                this.flySpeed = flySpeed;
                this.movAccel = movAccel;
                this.airborneAccelMult = airborneAccelMult;
            }
        }
    }
}
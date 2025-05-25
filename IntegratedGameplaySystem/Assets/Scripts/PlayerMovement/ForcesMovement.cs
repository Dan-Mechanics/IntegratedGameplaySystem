using UnityEngine;
using System;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// refactor like all of this but #no overthinking
    /// </summary>
    public class ForcesMovement
    {
        /// <summary>
        /// !Coupling !!
        /// Make different to camerhandler and give better name.
        /// </summary>
        private CameraHandler.Tick currentTick;
        private readonly GroundedConfiguration groundedConfig;
        private readonly Settings settings;
        private readonly References player;
        private bool isGrounded;

        public ForcesMovement(GroundedConfiguration groundedConfig, Settings settings, References player)
        {
            this.groundedConfig = groundedConfig;
            this.settings = settings;
            this.player = player;
        }

        /// <summary>
        /// This code sux. Fit it!
        /// REFACTOR !! --> consider splitting into smaller things.
        /// </summary>
        public void DoMovement(float vert, float hori) 
        {
            isGrounded = GetIsGrounded(groundedConfig, player.trans.position);
            //player.rb.velocity = Vector3.ClampMagnitude(player.rb.velocity, isGrounded ? settings.runSpeed : settings.flySpeed);
            
            float accel = isGrounded ? settings.movAccel : settings.movAccel * settings.airborneAccelMult;
            Vector3 mov = GetMovement(vert, hori, player);

            Vector3 flatVel = player.rb.velocity;
            flatVel.y = 0f;
            float mag = flatVel.magnitude;

            if (mag < settings.walkSpeed)
            {
                player.rb.AddForce(Vector3.ClampMagnitude(accel * Time.fixedDeltaTime * mov, settings.walkSpeed - mag), ForceMode.VelocityChange);
            }
            else if (isGrounded)
            {
                player.rb.AddForce(Vector3.ClampMagnitude(accel * Time.fixedDeltaTime * -flatVel.normalized, mag - settings.walkSpeed), ForceMode.VelocityChange);
            }

            Vector3 counterMovement = accel * Time.fixedDeltaTime * settings.airborneAccelMult * -(flatVel.normalized - mov);
            if (mag != 0f && counterMovement.magnitude > mag)
                counterMovement = -flatVel;

            player.rb.AddForce(counterMovement, ForceMode.VelocityChange);
        }

        public CameraHandler.Tick GetTick() 
        {
            player.rb.velocity = Vector3.ClampMagnitude(player.rb.velocity, isGrounded ? settings.runSpeed : settings.flySpeed);
            currentTick.Set(player.eyes.position, player.rb.velocity, Time.time);
            return currentTick;
        }

        private Vector3 GetMovement(float vert, float hori, References references) 
        {
            Vector3 mov = Vector3.zero;

            mov += references.trans.right * hori;
            mov += references.trans.forward * vert;
            mov.Normalize();

            return mov;
        }

        private bool GetIsGrounded(GroundedConfiguration config, Vector3 playerPosition)
        {
            RaycastHit[] hits = Physics.SphereCastAll(playerPosition, config.radius, Vector3.down, config.reach, config.mask, QueryTriggerInteraction.Ignore);

            foreach (RaycastHit hit in hits)
            {
                if (hit.distance == 0f)
                    continue;

                if (Vector3.Angle(Vector3.up, hit.normal) <= config.maxAngle)
                    return true;
            }

            return false;
        }

        [Serializable]
        public class GroundedConfiguration 
        {
            public LayerMask mask;
            public float radius;
            public float reach;
            public float maxAngle;
        }

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

        [Serializable]
        public class Settings 
        {
            public float walkSpeed;
            public float runSpeed;
            public float flySpeed;
            public float movAccel;
            public float airborneAccelMult;
        }
    }
}
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
        private readonly MovementSettings settings;
        private readonly GroundedConfiguration groundedConfig;

        private Transform trans;
        private Transform eyes;
        private Rigidbody rb;
        private bool isGrounded;

        public ForcesMovement(Transform trans, Transform eyes, Rigidbody rb)
        {
            this.trans = trans;
            this.eyes = eyes;
            this.rb = rb;
            
            IAssetService assets = ServiceLocator<IAssetService>.Locate();

            groundedConfig = assets.FindAsset<GroundedConfiguration>("grounded_config");
            settings = assets.FindAsset<MovementSettings>("movement_settings");
        }

        /// <summary>
        /// This code sux. Fit it!
        /// REFACTOR !! --> consider splitting into smaller things.
        /// </summary>
        public void DoMovement(float vert, float hori) 
        {
            isGrounded = GetIsGrounded(groundedConfig, trans.position);

            float accel = isGrounded ? settings.movAccel : settings.movAccel * settings.airborneAccelMult;
            Vector3 mov = GetMovement(vert, hori, trans);

            Vector3 flatVel = rb.velocity;
            flatVel.y = 0f;
            float mag = flatVel.magnitude;

            if (mag < settings.walkSpeed)
            {
                rb.AddForce(Vector3.ClampMagnitude(accel * Time.fixedDeltaTime * mov, settings.walkSpeed - mag), ForceMode.VelocityChange);
            }
            else if (isGrounded)
            {
                rb.AddForce(Vector3.ClampMagnitude(accel * Time.fixedDeltaTime * -flatVel.normalized, mag - settings.walkSpeed), ForceMode.VelocityChange);
            }

            Vector3 counterMovement = accel * Time.fixedDeltaTime * settings.airborneAccelMult * -(flatVel.normalized - mov);
            if (mag != 0f && counterMovement.magnitude > mag)
                counterMovement = -flatVel;

            rb.AddForce(counterMovement, ForceMode.VelocityChange);
        }

        public CameraHandler.Tick GetTick() 
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, isGrounded ? settings.runSpeed : settings.flySpeed);
            currentTick.Set(eyes.position, rb.velocity, Time.time);
            return currentTick;
        }

        private Vector3 GetMovement(float vert, float hori, Transform orientedBody) 
        {
            Vector3 mov = Vector3.zero;

            mov += orientedBody.right * hori;
            mov += orientedBody.forward * vert;
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
    }
}
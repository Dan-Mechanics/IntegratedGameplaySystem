using UnityEngine;
using System;

namespace IntegratedGameplaySystem
{
    public interface ISpeedSource 
    {
        float GetSpeed();
    }

    /// <summary>
    /// refactor like all of this but #no overthinking
    /// </summary>
    public class ForcesMovement
    {
        /// <summary>
        /// !Coupling !!
        /// Make different to camerhandler and give better name.
        /// </summary>
        private readonly PlayerSettings settings;
        private readonly Transform trans;
        private readonly Transform eyes;
        private readonly Rigidbody rb;

        private ISpeedSource speedSource;

        /// <summary>
        /// DEPENDACY !!! AHHH FIX !!
        /// </summary>
        private CameraMotionSnapshot snapshot;
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

        /// <summary>
        /// This code sux. Fit it!
        /// REFACTOR !! --> consider splitting into smaller things.
        /// </summary>
        public void DoMovement(float vert, float hori)
        {
            isGrounded = GetIsGrounded();
            //Debug.Log(isGrounded);
            float accel = isGrounded ? settings.movAccel : settings.movAccel * settings.accelMult;
            Vector3 mov = GetMovement(vert, hori, trans);

            Vector3 flatVel = rb.velocity;
            flatVel.y = 0f;

            float mag = flatVel.magnitude;
            
            rb.AddForce(accel * Time.fixedDeltaTime * mov, ForceMode.VelocityChange);

            Vector3 counterMovement = accel * Time.fixedDeltaTime * settings.accelMult * -(flatVel.normalized - mov);
            if (mag >= 0f && counterMovement.magnitude > mag)
                counterMovement = -flatVel;

            rb.AddForce(counterMovement, ForceMode.VelocityChange);
        }

        /*public void DoMovement(float vert, float hori) 
        {
            isGrounded = GetIsGrounded();

            float accel = isGrounded ? settings.movAccel : settings.movAccel * settings.accelMult;
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

            Vector3 counterMovement = accel * Time.fixedDeltaTime * settings.accelMult * -(flatVel.normalized - mov);
            if (mag != 0f && counterMovement.magnitude > mag)
                counterMovement = -flatVel;

            rb.AddForce(counterMovement, ForceMode.VelocityChange);
        }*/

        public CameraMotionSnapshot GetSnapshot() 
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, speedSource.GetSpeed());
            snapshot.Set(eyes.position, rb.velocity, Time.time);
            return snapshot;
        }

        private Vector3 GetMovement(float vert, float hori, Transform trans) 
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
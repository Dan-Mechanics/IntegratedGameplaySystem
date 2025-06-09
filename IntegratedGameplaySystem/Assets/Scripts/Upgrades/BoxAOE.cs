using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    [Serializable]
    public struct BoxAOE
    {
        public Vector3 halfExtents;
        public LayerMask mask;
        public QueryTriggerInteraction interaction;
        [HideInInspector] public Collider[] colliders;

        public void Setup(int maxExpectedColliders)
        {
            colliders = new Collider[maxExpectedColliders];
        }

        public int GetCollidersHitSphere(Vector3 position)
        {
            return Physics.OverlapBoxNonAlloc(position, halfExtents, colliders, Quaternion.identity, mask, interaction);
        }
    }
}

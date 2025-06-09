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

        public int GetCollidersHitBox(Vector3 position)
        {
            //return Physics.OverlapBox(position, halfExtents, Quaternion.identity, mask, interaction);
            return Physics.OverlapBoxNonAlloc(position, halfExtents, colliders, Quaternion.identity, mask, interaction);
        }
    }
}

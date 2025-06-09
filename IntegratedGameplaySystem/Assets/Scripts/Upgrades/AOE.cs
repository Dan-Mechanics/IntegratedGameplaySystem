using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    [Serializable]
    public struct AOE
    {
        public float range;
        public LayerMask mask;
        public QueryTriggerInteraction interaction;
        [HideInInspector] public Collider[] colliders;

        public void Setup(int maxExpectedColliders) 
        {
            colliders = new Collider[maxExpectedColliders];
        }

        public int GetCollidersHitSphere(Vector3 position) 
        {
            return Physics.OverlapSphereNonAlloc(position, range, colliders, mask, interaction);
        }
    }
}

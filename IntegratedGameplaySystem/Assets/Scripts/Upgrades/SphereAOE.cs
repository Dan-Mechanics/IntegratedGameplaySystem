using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    [Serializable]
    public class SphereAOE : OverlapNonAlloc
    {
        public float range;
        /*public LayerMask mask;
        public QueryTriggerInteraction interaction;
        [HideInInspector] public Collider[] colliders;*/

        /*public void Setup(int maxExpectedColliders) 
        {
            colliders = new Collider[maxExpectedColliders];
        }*/

        public override int GetColliderCount(Vector3 position) 
        {
            return Physics.OverlapSphereNonAlloc(position, range, colliders, mask, interaction);
        }
    }
}

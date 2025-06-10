using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    [Serializable]
    public abstract class OverlapNonAlloc 
    {
        public LayerMask mask;
        public QueryTriggerInteraction interaction;
        [HideInInspector] public Collider[] colliders;

        public void Setup(int maxExpectedColliders)
        {
            colliders = new Collider[maxExpectedColliders];
        }

        public abstract int GetColliderCount(Vector3 position);
    }
    
    [Serializable]
    public class BoxAOE : OverlapNonAlloc
    {
        //public Func<int, Vector3> OnOverlap;
        
        
        public Vector3 halfExtents;
        /*public LayerMask mask;
        public QueryTriggerInteraction interaction;
        [HideInInspector] public Collider[] colliders;

        public void Setup(int maxExpectedColliders)
        {
            colliders = new Collider[maxExpectedColliders];
        }*/

        public override int GetColliderCount(Vector3 position)
        {
            //return Physics.OverlapBox(position, halfExtents, Quaternion.identity, mask, interaction);
            return Physics.OverlapBoxNonAlloc(position, halfExtents, colliders, Quaternion.identity, mask, interaction);
        }
    }
}

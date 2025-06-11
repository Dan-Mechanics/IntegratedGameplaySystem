using UnityEngine;

namespace IntegratedGameplaySystem
{
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
}

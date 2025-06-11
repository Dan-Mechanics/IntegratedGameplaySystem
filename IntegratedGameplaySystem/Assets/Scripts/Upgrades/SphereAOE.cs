using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    [Serializable]
    public class SphereAOE : OverlapNonAlloc
    {
        public float range;

        public override int GetColliderCount(Vector3 position) 
        {
            return Physics.OverlapSphereNonAlloc(position, range, colliders, mask, interaction);
        }
    }
}

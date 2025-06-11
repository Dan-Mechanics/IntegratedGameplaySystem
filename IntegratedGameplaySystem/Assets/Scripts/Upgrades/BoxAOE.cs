using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    [Serializable]
    public class BoxAOE : OverlapNonAlloc
    {
        public Vector3 halfExtents;

        public override int GetColliderCount(Vector3 position)
        {
            return Physics.OverlapBoxNonAlloc(position, halfExtents, colliders, Quaternion.identity, mask, interaction);
        }
    }
}

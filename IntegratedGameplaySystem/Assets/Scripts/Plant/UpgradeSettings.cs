using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem 
{
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(UpgradeSettings), fileName = "New " + nameof(UpgradeSettings))]
    public class UpgradeSettings : ScriptableObject
    {
        public UpgradeValues sprinkler;
        public UpgradeValues grenade;

        public GameObject grenadeEffect;
    }
}
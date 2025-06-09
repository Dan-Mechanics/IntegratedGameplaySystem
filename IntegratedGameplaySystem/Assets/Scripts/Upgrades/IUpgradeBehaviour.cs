using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public interface IUpgradeBehaviour : IStartable, IDisposable
    {
        public UpgradeCommonality Upgrade { get; set; }
    }
}
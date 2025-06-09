using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public interface IUpgradeBehaviour : IStartable, IDisposable
    {
        UpgradeCommonality Upgrade { get; set; }
    }
}
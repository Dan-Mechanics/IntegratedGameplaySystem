using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class RunningShoes : IUpgradeBehaviour
    {
        private readonly UpgradeSettings settings;
        private readonly ForcesMovement movement;

        public UpgradeCommonality Upgrade { get; set; }

        public RunningShoes(UpgradeCommonality Upgrade, UpgradeSettings settings, ForcesMovement movement)
        {
            this.settings = settings;
            this.Upgrade = Upgrade;
        }

        public void Start()
        {
            Upgrade.OnBuy += Buy;
        }

        private void Buy()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Upgrade.OnBuy -= Buy;
        }

    }
}
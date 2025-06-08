using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class RunningShoes : IUpgradeBehaviour, ISpeedMod
    {
        public UpgradeCommonality Upgrade { get; set; }
        private readonly FirstPersonPlayer movement;
        private readonly UpgradeSettings settings;

        public RunningShoes(UpgradeCommonality Upgrade, UpgradeSettings settings, FirstPersonPlayer movement)
        {
            this.Upgrade = Upgrade;
            this.settings = settings;
            this.movement = movement;
        }

        public void Start()
        {
            Upgrade.OnBuy += Buy;
        }

        private void Buy()
        {
            movement.Movement.SetSpeedMod(this);
        }

        public void Dispose()
        {
            Upgrade.OnBuy -= Buy;
        }

        public float GetSpeedBoost()
        {
            return settings.runningShoesBoost;
        }
    }
}
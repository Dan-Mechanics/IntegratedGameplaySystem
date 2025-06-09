using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class RunningShoes : IUpgradeBehaviour
    {
        public UpgradeCommonality Upgrade { get; set; }

        private readonly ForcesMovement movement;
        private readonly RunningShoesSettings settings;

        public RunningShoes(UpgradeCommonality Upgrade, RunningShoesSettings settings, ForcesMovement movement)
        {
            this.Upgrade = Upgrade;
            this.settings = settings;
            this.movement = movement;
        }

        public void Start()
        {
            Upgrade.OnBuy += PutOnShoes;
        }

        private void PutOnShoes()
        {
            movement.SetSpeedSource(settings);
        }

        public void Dispose()
        {
            Upgrade.OnBuy -= PutOnShoes;
        }
    }
}
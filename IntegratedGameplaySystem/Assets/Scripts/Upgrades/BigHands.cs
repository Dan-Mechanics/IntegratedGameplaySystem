using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class BigHands : IUpgradeBehaviour
    {
        public UpgradeCommonality Upgrade { get; set; }

        private readonly AllUpgradeSettings settings;
        private readonly Hand hand;

        public BigHands(UpgradeCommonality Upgrade, AllUpgradeSettings settings, Hand hand)
        {
            this.settings = settings;
            this.Upgrade = Upgrade;
            this.hand = hand;
        }

        public void Start()
        {
            Upgrade.OnBuy += Upgrade_OnBuy;
        }

        private void Upgrade_OnBuy()
        {
            hand.isBoosted = true;
        }

        public void Dispose()
        {
            Upgrade.OnBuy -= Upgrade_OnBuy;
        }

    }
}
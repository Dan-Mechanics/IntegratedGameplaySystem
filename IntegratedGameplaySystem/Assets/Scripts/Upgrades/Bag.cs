using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class Bag : IUpgradeBehaviour
    {
        public UpgradeCommonality Upgrade { get; set; }

        private readonly BagSettings settings;
        private readonly Hand hand;

        public Bag(UpgradeCommonality Upgrade, BagSettings settings, Hand hand)
        {
            this.settings = settings;
            this.Upgrade = Upgrade;
            this.hand = hand;
        }

        public void Start()
        {
            Upgrade.OnBuy += EquipBag;
        }

        private void EquipBag()
        {
            hand.SetMaxStackSource(settings);
        }

        public void Dispose()
        {
            Upgrade.OnBuy -= EquipBag;
        }
    }
}
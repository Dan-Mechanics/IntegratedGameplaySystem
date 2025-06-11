using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class Sprinkler : IUpgradeBehaviour
    {
        public UpgradeCommonality Upgrade { get; set; }
        private readonly IWorldService world;
        private readonly SprinklerSettings settings;

        public Sprinkler(UpgradeCommonality Upgrade, SprinklerSettings settings)
        {
            this.settings = settings;
            this.Upgrade = Upgrade;
            world = ServiceLocator<IWorldService>.Locate();
        }

        private bool isSubscribed;

        public void Start() 
        {
            Upgrade.OnBuy += SubscribeToTicks;
        }

        private void SubscribeToTicks()
        {
            EventManager.AddListener(Occasion.LateTick, LateTick);
            isSubscribed = true;
        }

        public void Dispose()
        {
            if (isSubscribed)
                EventManager.RemoveListener(Occasion.LateTick, LateTick);

            Upgrade.OnBuy -= SubscribeToTicks;
        }

        private void LateTick()
        {
            int count = settings.overlapBox.GetColliderCount(Upgrade.Position);

            for (int i = 0; i < count; i++)
            {
                world.GetComponent<IWaterable>(settings.overlapBox.colliders[i].transform).Water();
            }
        }
    }
}
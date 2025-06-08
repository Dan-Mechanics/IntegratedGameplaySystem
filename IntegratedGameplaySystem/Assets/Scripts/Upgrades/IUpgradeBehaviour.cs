using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /*public interface IPurchasable : IInteractable, IHoverable
    {
        public event Func<int, bool> OnCanBuy;
        public event Action OnBuy;
    }*/

    public interface IUpgradeBehaviour : IStartable, IDisposable
    {
        public UpgradeCommonality Upgrade { get; set; }
    }

    public class UpgradeCommonality : IInteractable, IHoverable 
    {
        public Vector3 Position => button.transform.position;
        //public bool HasBeenBought => hasBeenBought;

        public event Action OnBuy;
        public event Func<int, bool> OnCanBuy;

        private readonly UpgradeValues values;
        private readonly IWorldService world;
        private readonly GameObject button;

       // private bool hasBeenBought;

        public UpgradeCommonality(Vector3 position, UpgradeValues values)
        {
            this.values = values;
            button = Utils.SpawnPrefab(values.buttonPrefab);
            button.transform.position = position + values.buttonPrefab.transform.position;

            world = ServiceLocator<IWorldService>.Locate();
            world.Add(button, this);
        }

        public string GetHoverTitle()
        {
            if (!OnCanBuy.Invoke(values.cost))
                return $"Can't afford {values.name.ToLower()} yet!";

            return $"Buy {values.name.ToLower()} for ${values.cost}";
        }

        public void Interact()
        {
            if (!OnCanBuy.Invoke(values.cost))
                return;

            if (values.singlePurchase)
                world.Remove(button);

          //  hasBeenBought = true;
            EventManager<int>.RaiseEvent(Occasion.LoseMoney, values.cost);
            OnBuy?.Invoke();
        }
    }
}
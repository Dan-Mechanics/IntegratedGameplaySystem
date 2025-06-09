using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class UpgradeCommonality : IInteractable, IHoverable 
    {
        public Vector3 Position => button.transform.position;
        public bool HasBeenBought => hasBeenBought;

        public event Action OnBuy;
        public event Func<int, bool> OnCanBuy;

        private readonly IUpgradeValues values;
        private readonly IWorldService world;
        private readonly GameObject button;

        private bool hasBeenBought;

        public UpgradeCommonality(Vector3 position, IUpgradeValues values)
        {
            this.values = values;
            button = Utils.SpawnPrefab(values.ButtonPrefab);
            button.transform.position = position + values.ButtonPrefab.transform.position + values.Offset;
            button.GetComponent<MeshRenderer>().material.color = values.Color;

            world = ServiceLocator<IWorldService>.Locate();
            world.Add(button, this);
        }

        public string GetHoverTitle()
        {
            if (!OnCanBuy.Invoke(values.Cost))
                return $"Can't afford {values.Name.ToLower()} yet!";

            return $"Buy {values.Name.ToLower()} for ${values.Cost}";
        }

        public void Interact()
        {
            if (!OnCanBuy.Invoke(values.Cost))
                return;

            if (values.SinglePurchase)
            {
                if (hasBeenBought)
                    return;

                world.Remove(button);
            }

            hasBeenBought = true;
            EventManager<int>.RaiseEvent(Occasion.LoseMoney, values.Cost);
            OnBuy?.Invoke();
        }
    }
}
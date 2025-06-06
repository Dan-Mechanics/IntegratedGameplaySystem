using System.Collections.Generic;
using UnityEngine;
using System;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// is there a way to make this use a pattern?
    /// Commonalityies:
    /// price
    /// interactable
    /// afford
    /// 
    /// You could say that the sprinkler lasts X amount of secodns right.
    /// </summary>
    public class PermaUpgrade : IInteractable, IHoverable
    {
        public event Action OnBuy;
        public event Func<int, bool> OnCanAfford;

        private readonly string name;
        private readonly int cost;
        private readonly GameObject go;
        private readonly IWorldService world;

        private bool hasBought;

        public PermaUpgrade(string name, int cost, GameObject go, IWorldService world)
        {
            this.name = name;
            this.cost = cost;
            this.world = world;
            this.go = go;

            world.Add(go, this);
        }

        public bool GetHasBought() => hasBought;

        public string GetHoverTitle()
        {
            if (hasBought)
                return string.Empty;

            if (!OnCanAfford.Invoke(cost))
                return "Can't afford yet!";

            return $"{name} upgrade for ${cost}";
        }

        //public bool GetCanAfford() => OnCanAfford.Invoke(cost);

        public void Interact()
        {
            if (hasBought)
                return;

            if (!OnCanAfford.Invoke(cost))
                return;

            EventManager<int>.RaiseEvent(Occasion.LoseMoney, cost);
            hasBought = true;
            OnBuy?.Invoke();
            world.Remove(go);
        }
    }

   /* public class ConsumableUpgrade : IInteractable, IHoverable
    {
        public event Action OnBuy;
        public event Func<int, bool> OnCanAfford;

        private readonly string name;
        private readonly int cost;

        public ConsumableUpgrade(string name, int cost, GameObject go, IWorldService world)
        {
            this.name = name;
            this.cost = cost;
            world.Add(go, this);
        }

        public string GetHoverTitle()
        {
            if (!OnCanAfford.Invoke(cost))
                return "Can't afford yet!";

            return $"buy {name} for ${cost}";
        }

        public void Interact()
        {
            if (!OnCanAfford.Invoke(cost))
                return;

            EventManager<int>.RaiseEvent(Occasion.LoseMoney, cost);
            OnBuy?.Invoke();
        }
    }*/

}
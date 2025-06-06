using System;
using UnityEngine;

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
    [Serializable]
    public class UpgradeValues 
    {
        public string name;
        public int cost;
        public GameObject buttonPrefab;
    }

    public abstract class UpgradeBase : IInteractable, IHoverable 
    {

    }

    /// <summary>
    /// See how to reduce the repeating between these ??
    /// Make interface seems best.
    /// </summary>
    public class PermanentUpgrade : IInteractable, IHoverable
    {
        public event Func<int, bool> OnCanAfford;

        public UpgradeValues values;

        private IWorldService world;
        private GameObject button;
        private bool hasBought;

        public void Setup(UpgradeValues values, IWorldService world, Vector3 offset)
        {
            this.world = world;
            this.values = values;

            button = Utils.SpawnPrefab(values.buttonPrefab);
            button.transform.position += offset;

            world.Add(button, this);
        }

        public bool GetHasBought() => hasBought;

        public string GetHoverTitle()
        {
            if (hasBought)
                return string.Empty;

            if (!OnCanAfford.Invoke(values.cost))
                return "Can't afford yet!";

            return $"{values.name} upgrade for ${values.cost}";
        }

        public void Interact()
        {
            if (hasBought)
                return;

            if (!OnCanAfford.Invoke(values.cost))
                return;

            EventManager<int>.RaiseEvent(Occasion.LoseMoney, values.cost);
            hasBought = true;
            world.Remove(button);
        }
    }

    public class RepeatableUpgrade : IInteractable, IHoverable
    {
        public event Action OnBuy;
        public event Func<int, bool> OnCanAfford;

        public UpgradeValues values;
        private GameObject button;

        public void Setup(UpgradeValues values, IWorldService world, Vector3 offset)
        {
            this.values = values;

            button = Utils.SpawnPrefab(values.buttonPrefab);
            button.transform.position += offset;

            world.Add(button, this);
        }

        public string GetHoverTitle()
        {
            if (!OnCanAfford.Invoke(values.cost))
                return "Can't afford yet!";

            return $"{values.name} for ${values.cost}";
        }

        public void Interact()
        {
            if (!OnCanAfford.Invoke(values.cost))
                return;

            EventManager<int>.RaiseEvent(Occasion.LoseMoney, values.cost);
            OnBuy?.Invoke();
        }
    }
}
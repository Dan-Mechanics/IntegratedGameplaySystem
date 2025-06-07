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

    /// <summary>
    /// LOOK INTO INTERFACCCCCCEEEE
    /// </summary>
    public abstract class UpgradeBase : IInteractable, IHoverable 
    {
        public Action OnBuy;
        public Func<int, bool> OnCanAfford;

        protected UpgradeValues values;
        protected GameObject button;

        /// <summary>
        /// Do this before Setup().
        /// </summary>
        public void SetValues(UpgradeValues values) => this.values = values;
        public abstract void Setup(IWorldService world, Vector3 offset);
        public abstract string GetHoverTitle();
        public abstract void Interact();
    }

    /// <summary>
    /// See how to reduce the repeating between these ??
    /// Make interface ??? ========
    /// Make interface seems best.
    /// </summary>
    [Serializable]
    public class PermanentUpgrade : UpgradeBase
    {
        private IWorldService world;
        private bool hasBought;

        public override void Setup(IWorldService world, Vector3 offset)
        {
            if (values == null) 
            {
                Debug.LogError("SetValues() first please.");
                return;
            }
            
            this.world = world;

            button = Utils.SpawnPrefab(values.buttonPrefab);
            button.transform.position += offset;

            world.Add(button, this);
        }

        public bool GetHasBought() => hasBought;

        public override string GetHoverTitle()
        {
            if (hasBought)
                return string.Empty;

            if (!OnCanAfford.Invoke(values.cost))
                return "Can't afford yet!";

            return $"{values.name} upgrade for ${values.cost}";
        }

        public override void Interact()
        {
            if (hasBought)
                return;

            if (!OnCanAfford.Invoke(values.cost))
                return;

            OnBuy?.Invoke();
            EventManager<int>.RaiseEvent(Occasion.LoseMoney, values.cost);
            hasBought = true;
            world.Remove(button);
        }
    }

    [Serializable]
    public class RepeatableUpgrade : UpgradeBase
    {
        //public event Action OnBuy;

        public override void Setup(IWorldService world, Vector3 offset)
        {
            button = Utils.SpawnPrefab(values.buttonPrefab);
            button.transform.position += offset;

            world.Add(button, this);
        }

        public override string GetHoverTitle()
        {
            if (!OnCanAfford.Invoke(values.cost))
                return "Can't afford yet!";

            return $"{values.name} for ${values.cost}";
        }

        public override void Interact()
        {
            if (!OnCanAfford.Invoke(values.cost))
                return;

            EventManager<int>.RaiseEvent(Occasion.LoseMoney, values.cost);
            OnBuy?.Invoke();
        }
    }
}
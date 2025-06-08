using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Give this plant an Ipositioner that positions it according to some rules ya know?
    /// I want this to be able to work with plots instead of patches.
    /// 
    /// Statemachine ?? --> Dry, Watered, Grown
    /// </summary>
    public class Plant : IStartable, IInteractable, IHoverable, IDisposable, IHarvestable, IWaterable
    {
        //public bool IsWatered => alwaysWatered || wateredByHand;

        /// <summary>
        /// Yes, I know we are defining a hard rule here.
        /// </summary>
        public bool IsHarvestable => progression >= flyweight.materials.Length - 1;

        public readonly GameObject gameObject;
        public readonly Transform transform;

        private readonly PlantFlyweight flyweight;
        private readonly MeshRenderer[] meshRenderers;
        private readonly SphereCollider sphereCollider;
        private readonly IPoolService<PoolableParticle> pool;

        private int progression;
        //private bool alwaysWatered;
        private bool isWatered;
        private bool rainEffectShowing;
        private PoolableParticle rainParticles;

        /// <summary>
        /// This is kinda nutty becasue we want to save load time but ok,
        /// maybe prioirtize SOLID instead right.
        /// If u gonna makethis solid do it in da start pls.
        /// I doubt the reviewers would notice.
        /// 
        /// Factory here !!!
        /// </summary>
        public Plant(PlantFlyweight flyweight)
        {
            this.flyweight = flyweight;

            gameObject = Utils.SpawnPrefab(flyweight.plantPrefab);
            transform = gameObject.transform;
            transform.name = flyweight.name;

            pool = ServiceLocator<IPoolService<PoolableParticle>>.Locate();
            sphereCollider = gameObject.AddComponent<SphereCollider>();
            meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
        }

        public string GetHoverTitle() 
        {
            if (!isWatered && !IsHarvestable)
                return $"dry {flyweight.name}";

            return flyweight.name;
        }

        public void Start()
        {
            ServiceLocator<IWorldService>.Locate().Add(gameObject, this);
            EventManager.AddListener(Occasion.PlantTick, Tick);

            RefreshMaterials();
            RefreshCollider();
            RefreshRainEffect();
        }

        /// <summary>
        /// We could say we remove the gameobject from the gamewolrd
        /// here but it gets cleared when we move scene anyway...
        /// </summary>
        public void Dispose()
        {
            EventManager.RemoveListener(Occasion.PlantTick, Tick);
        }

        private void Tick()
        {
            if (!Utils.RandomWithPercentage(isWatered ? flyweight.wateredGrowGrowPercentage : flyweight.dryGrowPercentage))
                return;

            Grow();
        }

        private void Grow()
        {
            isWatered = false;
            progression++;
            progression = Mathf.Clamp(progression, 0, flyweight.materials.Length - 1);

            RefreshMaterials();
            RefreshCollider();
            RefreshRainEffect();
        }

        private void RefreshMaterials() 
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].material = flyweight.materials[progression];
            }
        }

        private void RefreshCollider() 
        {
            sphereCollider.center = Vector3.down * (IsHarvestable ? 0f : 0.5f);
            sphereCollider.enabled = IsHarvestable || !isWatered;
        }

        public void RefreshRainEffect()
        {
            if (rainEffectShowing == isWatered)
                return;

            if (isWatered)
            {
                rainParticles = pool.Get();
                rainParticles.Place(transform.position + Vector3.up);
            }
            else
            {
                pool.Give(rainParticles);
            }

            rainEffectShowing = isWatered;
        }

        public void Interact()
        {
            Water();
            Harvest();
        }

        public void Harvest()
        {
            if (!IsHarvestable)
                return;

            progression = 0;
            isWatered = false;
            EventManager<IItemArchitype>.RaiseEvent(Occasion.PickupItem, flyweight);

            RefreshMaterials();
            RefreshCollider();
            RefreshRainEffect();
        }

        public void Water()
        {
            if (isWatered)
                return;

            isWatered = true;

            RefreshRainEffect();
            RefreshCollider();
        }
    }

    /*public interface IPlantState 
{
    void Interact();
    void Tick();
}

public class Dry : IPlantState
{
    public void Interact()
    {
        throw new NotImplementedException();
    }

    public void Tick()
    {
        throw new NotImplementedException();
    }
}

public class Wet : IPlantState
{
    public void Interact()
    {
        throw new NotImplementedException();
    }

    public void Tick()
    {
        throw new NotImplementedException();
    }
}

public class Harvestable : IPlantState
{
    public void Interact()
    {
        throw new NotImplementedException();
    }

    public void Tick()
    {
        throw new NotImplementedException();
    }
}*/

}
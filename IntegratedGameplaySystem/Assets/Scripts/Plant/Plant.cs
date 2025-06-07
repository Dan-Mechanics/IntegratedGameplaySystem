using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public interface IPlantState 
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
    }

    /// <summary>
    /// Give this plant an Ipositioner that positions it according to some rules ya know?
    /// I want this to be able to work with plots instead of patches.
    /// 
    /// Statemachine ?? --> Dry, Watered, Grown
    /// </summary>
    public class Plant : IStartable, IInteractable, IHoverable, IDisposable
    {
        public bool IsWatered => alwaysWatered || wateredByHand;
        public bool IsHarvestable => progression >= flyweight.materials.Length - 1;

        public readonly GameObject gameObject;
        public readonly Transform transform;

        // Composite this ??
        private readonly PlantFlyweight flyweight;
        private readonly MeshRenderer[] meshRenderers;
        private readonly SphereCollider sphereCollider;
        private readonly IPoolService<PoolableParticle> pool;

        private int progression;
        private bool alwaysWatered;
        private bool wateredByHand;
        private bool showingEffects;
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

            /*GameObject rain = Utils.SpawnPrefab(blueprint.rainPrefab);
            rain.transform.SetParent(transform);
            rain.transform.localPosition = blueprint.rainPrefab.transform.localPosition;
            rainEffect = rain.GetComponent<ParticleSystem>();*/

            pool = ServiceLocator<IPoolService<PoolableParticle>>.Locate();
            sphereCollider = gameObject.AddComponent<SphereCollider>();
            meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
        }

        public void SetAlwaysWatered(bool value) 
        {
            alwaysWatered = value;
            RefreshRainEffects();
        }

        public string GetHoverTitle() 
        {
            if (!IsWatered && !IsHarvestable)
                return $"dry {flyweight.name}";

            return flyweight.name;
        }

        public void Start()
        {
            ServiceLocator<IWorldService>.Locate().Add(gameObject, this);
            EventManager.AddListener(Occasion.Tick, Tick);

            RefreshAll();
        }

        /// <summary>
        /// We could say we remove the gameobject from the gamewolrd
        /// here but it gets cleared when we move scene anyway...
        /// </summary>
        public void Dispose()
        {
            EventManager.RemoveListener(Occasion.Tick, Tick);
        }

        private void Tick()
        {
            if (!Utils.OneIn(IsWatered ? flyweight.wateredGrowOdds : flyweight.growOdds))
                return;

            Grow();
        }

        private void Grow()
        {
            wateredByHand = false;
            progression++;
            progression = Mathf.Clamp(progression, 0, flyweight.materials.Length - 1);

            RefreshAll();
        }

        private void RefreshAll() 
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].material = flyweight.materials[progression];
            }

            sphereCollider.center = Vector3.down * (IsHarvestable ? 0f : 0.5f);
            sphereCollider.enabled = IsHarvestable || !IsWatered;

            RefreshRainEffects();
        }

        /// <summary>
        /// USE OBJECT POOL !!!!!!!!!!!!!!!!! COOL CODE TO BE FOUND THERE.
        /// </summary>
        public void RefreshRainEffects()
        {
            if (showingEffects == IsWatered)
                return;

            if (IsWatered)
            {
                rainParticles = pool.Get();
                rainParticles.Place(transform.position + Vector3.up);
            }
            else
            {
                pool.Give(rainParticles);
            }

            showingEffects = IsWatered;
        }

        /// <summary>
        /// We should not be able to interact with this if not full-grown
        /// but i dont do protective coding, i would rather instantly find bugs right.
        /// </summary>
        public void Interact()
        {
            if (IsHarvestable) 
            { 
                TryHarvest();
                return;
            }

            TryWaterManually();
        }

        public void TryHarvest()
        {
            if (!IsHarvestable)
                return;

            progression = 0;
            wateredByHand = false;
            EventManager<IItemArchitype>.RaiseEvent(Occasion.SetOrAddItem, flyweight);

            RefreshAll();
        }

        private void TryWaterManually() 
        {
            if (IsWatered)
                return;

            wateredByHand = true;
            RefreshRainEffects();
        }
    }
}
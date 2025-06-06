using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Give this plant an Ipositioner that positions it according to some rules ya know?
    /// I want this to be able to work with plots instead of patches.
    /// </summary>
    public class Plant : IStartable, IInteractable, IHoverable, IDisposable
    {
        public Func<bool> HasSprinkler;
        public bool IsWatered => intervalWater || HasSprinkler();
        public string HoverTitle => GetHovering();
        public bool IsHarvestable => progression >= blueprint.materials.Length - 1;

        public readonly GameObject gameObject;
        public readonly Transform transform;

        // Composite this ??
        private readonly PlantFlyweight blueprint;
        private readonly MeshRenderer[] meshRenderers;
        private readonly SphereCollider sphereCollider;
        private readonly ParticleSystem waterEffect;

        private int progression;
        private bool intervalWater;
        private bool showingEffects;

        /// <summary>
        /// This is kinda nutty becasue we want to save load time but ok,
        /// maybe prioirtize SOLID instead right.
        /// If u gonna makethis solid do it in da start pls.
        /// I doubt the reviewers would notice.
        /// 
        /// Factory here !!!
        /// </summary>
        public Plant(PlantFlyweight blueprint)
        {
            this.blueprint = blueprint;

            gameObject = Utils.SpawnPrefab(blueprint.plantPrefab);
            transform = gameObject.transform;
            transform.name = blueprint.name;

            GameObject rain = Utils.SpawnPrefab(blueprint.rainPrefab);
            rain.transform.SetParent(transform);
            rain.transform.localPosition = blueprint.rainPrefab.transform.localPosition;
            waterEffect = rain.GetComponent<ParticleSystem>();

            sphereCollider = gameObject.AddComponent<SphereCollider>();
            meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
        }

        public string GetHovering() 
        {
            if (!IsWatered && !IsHarvestable)
                return $"dry {blueprint.name}";

            return blueprint.name;
        }

        public void Start()
        {
            UpdateIntervalWatered(false);

            ServiceLocator<IWorldService>.Locate().Add(gameObject, this);
            EventManager.AddListener(Occasion.Tick, Tick);
            Refresh();
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
            UpdateEffects();

            if (!Utils.OneIn(IsWatered ? blueprint.wateredGrowOdds : blueprint.growOdds))
                return;

            UpdateIntervalWatered(false);
            progression++;
            progression = Mathf.Clamp(progression, 0, blueprint.materials.Length - 1);
            Refresh();
        }

        private void Refresh() 
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].material = blueprint.materials[progression];
            }

            sphereCollider.center = Vector3.down * (IsHarvestable ? 0f : 0.5f);
            sphereCollider.enabled = IsHarvestable || !IsWatered;
        }

        private void UpdateIntervalWatered(bool intervalWater)
        {
            this.intervalWater = intervalWater;
            UpdateEffects();
        }

        private void UpdateEffects()
        {
            if (showingEffects == IsWatered)
                return;

            if (IsWatered)
            {
                waterEffect.Play();
            }
            else
            {
                waterEffect.Stop();
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
                progression = 0;
                EventManager<IItemArchitype>.RaiseEvent(Occasion.SetOrAddItem, blueprint);
            }
            else if (!IsWatered)
            {
                UpdateIntervalWatered(true);
            }

            Refresh();
        }
    }
}
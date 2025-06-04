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
        public readonly GameObject gameObject;
        public readonly Transform transform;

        // Composite this ??
        private readonly PlantBlueprint blueprint;
        private readonly MeshRenderer[] meshRenderers;
        private readonly SphereCollider sphereCollider;
        private readonly ParticleSystem waterEffect;

        private int progression;
        private bool isWatered;

        public string Name => GetHovering();
        public bool IsHarvestable => progression >= blueprint.materials.Length - 1;

        /// <summary>
        /// This is kinda nutty becasue we want to save load time but ok,
        /// maybe prioirtize SOLID instead right.
        /// If u gonna makethis solid do it in da start pls.
        /// I doubt the reviewers would notice.
        /// 
        /// Factory here.
        /// </summary>
        public Plant(PlantBlueprint blueprint)
        {
            this.blueprint = blueprint;

            gameObject = Utils.SpawnPrefab(blueprint.plantPrefab);
            transform = gameObject.transform;
            transform.name = blueprint.name;

            GameObject rain = Utils.SpawnPrefab(blueprint.rainPrefab);
            rain.transform.SetParent(transform);
            rain.transform.localPosition = blueprint.rainPrefab.transform.localPosition;
            waterEffect = rain.GetComponent<ParticleSystem>();

            gameObject.layer = LayerMask.NameToLayer(blueprint.name);

            sphereCollider = gameObject.AddComponent<SphereCollider>();
            meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
        }

        public string GetHovering() 
        {
            if (!isWatered && !IsHarvestable)
                return $"dry {blueprint.name}";

            return blueprint.name;
        }

        public void Start()
        {
            UpdateWatered(false);

            ServiceLocator<IWorldService>.Locate().Add(gameObject, this);
            EventManager.AddListener(Occasion.Tick, Tick);
            Refresh();
        }

        public void Dispose()
        {
            EventManager.RemoveListener(Occasion.Tick, Tick);
        }

        private void Tick()
        {
            if (!Utils.OneIn(isWatered ? blueprint.wateredGrowOdds : blueprint.growOdds))
                return;

            UpdateWatered(false);
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

            sphereCollider.enabled = IsHarvestable || !isWatered;
        }

        /// <summary>
        /// Use events ??
        /// </summary>
        /// <param name="water"></param>
        private void UpdateWatered(bool water)
        {
            isWatered = water;

            if (isWatered)
            {
                waterEffect.Play();
            }
            else
            {
                waterEffect.Stop();
            }
        }

        /// <summary>
        /// We should not be able to interact with this if not full-grown
        /// but i dont do protective coding, i would rather instantly find bugs right.
        /// </summary>
        public void Interact()
        {
            if (progression >= blueprint.materials.Length - 1)
            {
                progression = 0;
                EventManager<IItem>.RaiseEvent(Occasion.EquipItem, blueprint);
            }
            else if (!isWatered)
            {
                UpdateWatered(true);
            }

            Refresh();
        }
    }
}
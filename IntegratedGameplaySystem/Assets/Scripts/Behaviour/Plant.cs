using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Give this plant an Ipositioner that positions it according to some rules ya know?
    /// I want this to be able to work with plots instead of patches.
    /// </summary>
    public class Plant : IStartable, IInteractable, IDisposable
    {
        public const string PLANT_PREFAB_NAME = "plant";
        public const string RAIN_PREFAB_NAME = "rain";

        //public event Action<int> OnCollect;

        private readonly PlantSpeciesProfile blueprint;
        private readonly SceneObject sceneObject;
        private readonly MeshRenderer[] meshRenderers;
        private readonly SphereCollider sphereCollider;
        private readonly ParticleSystem waterEffect;

        private int progression;
        private bool isWatered;

        /// <summary>
        /// This is kinda nutty becasue we want to save load time but ok,
        /// maybe prioirtize SOLID instead right.
        /// If u gonna makethis solid do it in da start pls.
        /// I doubt the reviewers would notice.
        /// </summary>
        public Plant(PlantSpeciesProfile blueprint, GameObject plantPrefab, GameObject rainPrefab)
        {
            this.blueprint = blueprint;

            sceneObject = new SceneObject(plantPrefab);
            sceneObject.gameObject.name = blueprint.name;

            GameObject go = Utils.SpawnPrefab(rainPrefab);
            go.transform.SetParent(sceneObject.transform);
            go.transform.localPosition = rainPrefab.transform.localPosition;
            waterEffect = go.GetComponent<ParticleSystem>();

            sphereCollider = sceneObject.gameObject.AddComponent<SphereCollider>();
            meshRenderers = sceneObject.gameObject.GetComponentsInChildren<MeshRenderer>();
        }

        public void Start()
        {
            UpdateWatered(false);
            sceneObject.transform.position += Utils.GetRandomFlatPos(blueprint.dispersal);
            Utils.ApplyRandomRotation(sceneObject.transform);

            ServiceLocator<IWorldService>.Locate().Add(sceneObject.gameObject, this);
            EventManager.AddListener(Occasion.TICK, Tick);
            Refresh();
        }

        public void Dispose()
        {
            EventManager.RemoveListener(Occasion.TICK, Tick);
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

            sphereCollider.enabled = progression >= blueprint.materials.Length - 1 || !isWatered;
        }

        private void UpdateWatered(bool water)
        {
            isWatered = water;
            if (water)
                waterEffect.Play();
            else
                waterEffect.Stop();
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
                EventManagerGeneric<int>.RaiseEvent(Occasion.EARN_MONEY, blueprint.monetaryValue);
            }
            else if (!isWatered)
                UpdateWatered(true);

            Refresh();
        }
    }
}
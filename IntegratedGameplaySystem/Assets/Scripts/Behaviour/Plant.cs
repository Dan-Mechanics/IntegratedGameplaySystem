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
        
        public event Action<bool> OnCollect;

        private readonly PlantSpeciesProfile blueprint;
        private readonly SceneObject sceneObject;
        private readonly MeshRenderer[] meshRenderers;
        private readonly SphereCollider sphereCollider;

        private int progression;
        private bool watered;

        /// <summary>
        /// This is kinda nutty becasue we want to save load time but ok,
        /// maybe prioirtize SOLID instead right.
        /// If u gonna makethis solid do it in da start pls.
        /// I doubt the reviewers would notice.
        /// </summary>
        public Plant(PlantSpeciesProfile blueprint)
        {
            this.blueprint = blueprint;

            sceneObject = new SceneObject(PLANT_PREFAB_NAME);
            sceneObject.gameObject.name = blueprint.name;

            // You could add some funny scale variation here in the MeshRenderer !!
            sphereCollider = sceneObject.gameObject.AddComponent<SphereCollider>();
            meshRenderers = sceneObject.gameObject.GetComponentsInChildren<MeshRenderer>();
        }

        public void Start()
        {
            sceneObject.transform.position += Utils.GetRandomFlatPos(blueprint.dispersal);

            sceneObject.transform.Rotate(Vector3.up * UnityEngine.Random.Range(0f, 360f), Space.Self);
            //sceneObject.transform.localScale = Vector3.one * Random.Range(0.9f, 1.1f);

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
            if (!Utils.OneIn(blueprint.growOdds))
                return;

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

            sphereCollider.enabled = progression >= blueprint.materials.Length - 1;
        }

        /// <summary>
        /// We should not be able to interact with this if not full-grown
        /// but i dont do protective coding, i would rather instantly find bugs right.
        /// </summary>
        public void Interact()
        {
            progression = 0;
            Refresh();

            OnCollect?.Invoke(watered);
        }
    }
}
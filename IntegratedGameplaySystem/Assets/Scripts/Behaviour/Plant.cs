using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Unique.
    /// </summary>
    public class Plant : IStartable, IInteractable, IDisposable
    {
        public const string PLANT_PREFAB_NAME = "plant";
        
        //public event Action<int> OnEarnMoney;
        
        /// <summary>
        /// flyweight.
        /// </summary>
        private PlantSpeciesProfile profile;
        private int progression;

        //private bool isWet = false; 
        private MeshRenderer[] meshRenderers;
        private SphereCollider sphereCollider;

        /// <summary>
        /// This is kinda nutty becasue we want to save load time but ok,
        /// maybe prioirtize SOLID instead right.
        /// If u gonna makethis solid do it in da start pls.
        /// I doubt the reviewers would notice.
        /// </summary>
        public Plant(PlantSpeciesProfile profile)
        {
            this.profile = profile;
            EventManager.AddListener(Occasion.TICK, Tick);
            /*GameObject go = Utils.SpawnPrefab(prefab);

            // You could add some funny scale variation hereo n the meshrenderer.

            sphereCollider = go.AddComponent<SphereCollider>();
            meshRenderers = go.GetComponentsInChildren<MeshRenderer>();
            Refresh();

            // we could ahve it that it gives refernece to this.
            // !PERFORMANCE
            ServiceLocator<IWorldService>.Locate().Add(go, this);*/
        }

        public void Start()
        {
            GameObject go = Utils.SpawnPrefab(ServiceLocator<IAssetService>.Locate().GetByAgreedName(PLANT_PREFAB_NAME));

            // You could add some funny scale variation hereo n the meshrenderer.

            sphereCollider = go.AddComponent<SphereCollider>();
            meshRenderers = go.GetComponentsInChildren<MeshRenderer>();
            Refresh();
            ServiceLocator<IWorldService>.Locate().Add(go, this);
        }

        public void Dispose()
        {
            EventManager.RemoveListener(Occasion.TICK, Tick);
        }

        private void Tick()
        {
            // 1 in 7 ?
            if (!Utils.OneIn(7))
                return;

            progression++;
            progression = Mathf.Clamp(progression, 0, profile.materials.Length - 1);
            Refresh();
        }

        private void Refresh() 
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].material = profile.materials[progression];
            }

            sphereCollider.enabled = progression >= profile.materials.Length - 1;
        }

        public void Interact()
        {
            // cant cut when full.
            /*if (progression < blueprint.materials.Length - 1)
                return;*/

            progression = 0;
            //OnEarnMoney?.Invoke(1 * (isWet ? 2 : 1));
            Refresh();
        }
    }
}
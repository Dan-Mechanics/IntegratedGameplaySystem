using System;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Unique.
    /// </summary>
    public class Plant : IInteractable
    {
        public event Action<int> OnEarnMoney;
        
        /// <summary>
        /// flyweight.
        /// </summary>
        private PlantBlueprint blueprint;
        private int progression;

        /// <summary>
        /// Still working on this.
        /// </summary>
        private bool isWet; // for example.
        private MeshRenderer[] meshRenderers;

        public Plant(PlantBlueprint blueprint, Transform transform)
        {
            this.blueprint = blueprint;

            EventManager.AddListener(Occasion.TICK, Tick);

            // You could add some funny scale variation hereo n the meshrenderer.

            meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
            RefreshVisuals();
        }

        public void Dispose()
        {
            EventManager.RemoveListener(Occasion.TICK, Tick);
        }

        /*public void Update() 
        {

        }*/

        private void Tick()
        {
            // 1 in 7 ?
            if (!Utils.OneIn(7))
                return;

            progression++;
            progression = Mathf.Clamp(progression, 0, blueprint.materials.Length - 1);
            RefreshVisuals();
        }

        private void RefreshVisuals() 
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].material = blueprint.materials[progression];
            }
        }

        public void Interact()
        {
            progression = 0;
            OnEarnMoney?.Invoke(1 * (isWet ? 2 : 1));
            RefreshVisuals();
        }
    }
}
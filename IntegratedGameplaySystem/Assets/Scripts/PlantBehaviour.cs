using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class PlantBehaviour
    {
        /*[Tooltip("One in ...")]
        public int growOddsPerTick;
        public Material[] materials;
        public float preferredPlantSpacing;*/
        private Transform transform;
        private PlotBehaviour plotBehaviour;

        private int progression;
        private MeshRenderer[] meshRenderers;

        public PlantBehaviour(Transform transform, PlotBehaviour plotBehaviour)
        {
            this.transform = transform;
            this.plotBehaviour = plotBehaviour;

            EventManager.AddListener(Occasion.TICK, Tick);

            meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
            RefreshVisuals();
        }

        public void Dispose()
        {
            EventManager.RemoveListener(Occasion.TICK, Tick);
        }

        /*private void Disperse()
        {
            Vector2 rand = Random.insideUnitCircle * plotBehaviour.preferredPlantSpacing;

            transform.position += Vector3.forward * rand.y;
            transform.position += Vector3.right * rand.x;
        }*/

        private void Tick()
        {
            // 1 in 7 ?
            if (!OneIn(7))
                return;

            progression++;
            progression = Mathf.Clamp(progression, 0, plotBehaviour.materials.Length - 1);
            RefreshVisuals();
        }

        private void RefreshVisuals() 
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].material = plotBehaviour.materials[progression];
            }
        }

        /// <summary>
        /// Export to Utils.cs
        /// </summary>
        private bool OneIn(int x) 
        {
            return Random.Range(0, x + 1) == 0;
        }
    }
}
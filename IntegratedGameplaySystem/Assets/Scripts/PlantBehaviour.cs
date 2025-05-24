using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// And this would then be where we have our like input handler classes and such. Component pattern perchange ??? !!
    /// </summary>
    [CreateAssetMenu(menuName = nameof(BaseBehaviour) + "/" + nameof(PlantBehaviour), fileName = "New " + nameof(PlantBehaviour))]
    public class PlantBehaviour : BaseBehaviour
    {
        [Tooltip("One in ...")]
        public int growOddsPerTick;
        public Material[] materials;
        public float preferredPlantSpacing;

        private int progression;
        private MeshRenderer[] meshRenderers;

        public override void Start()
        {
            base.Start();
            EventManager.AddListener(Occasion.TICK, Tick);

            meshRenderers = transform.GetComponentsInChildren<MeshRenderer>();
            RefreshVisuals();

            Disperse();
        }

        public override void Disable()
        {
            base.Disable();
            EventManager.RemoveListener(Occasion.TICK, Tick);
        }

        private void Disperse() 
        {
            Vector2 rand = Random.insideUnitCircle * preferredPlantSpacing;

            transform.position += Vector3.forward * rand.y;
            transform.position += Vector3.right * rand.x;
        }

        private void Tick()
        {
            // 1 in 7 ?
            if (!OneIn(7))
                return;

            progression++;
            progression = Mathf.Clamp(progression, 0, materials.Length - 1);
            RefreshVisuals();
        }

        private void RefreshVisuals() 
        {
            for (int i = 0; i < meshRenderers.Length; i++)
            {
                meshRenderers[i].material = materials[progression];
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
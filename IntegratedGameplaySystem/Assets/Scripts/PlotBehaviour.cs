using UnityEngine;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// And this would then be where we have our like input handler classes and such. Component pattern perchange ??? !!
    /// </summary>
    [CreateAssetMenu(menuName = nameof(BaseBehaviour) + "/" + nameof(PlotBehaviour), fileName = "New " + nameof(PlotBehaviour))]
    public class PlotBehaviour : BaseBehaviour
    {
        [Tooltip("One in ...")]
        public int growOddsPerTick;
        public Material[] materials;
       // public float preferredPlantSpacing;

        private readonly List<PlantBehaviour> plants = new List<PlantBehaviour>();
        /*private int progression;
        private MeshRenderer[] meshRenderers;*/

        public override void Start()
        {
            base.Start();

            for (int i = 0; i < transform.childCount; i++)
            {
                plants.Add(new PlantBehaviour(transform.GetChild(i), this));
            }
        }

        public override void Disable()
        {
            base.Disable();
            plants.ForEach(x => x.Dispose());
        }
    }
}
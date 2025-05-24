using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// And this would then be where we have our like input handler classes and such. Component pattern perchange ??? !!
    /// </summary>
    [CreateAssetMenu(menuName = nameof(BaseBehaviour) + "/" + nameof(PlotBehaviour), fileName = "New " + nameof(PlotBehaviour))]
    public class PlotBehaviour : BaseBehaviour
    {
        public int count;

        private readonly List<Test> plants = new List<Test>();

        public override void Start()
        {
            base.Start();

            for (int i = 0; i < count; i++)
            {
                plants.Add(new PlantBehaviour().Setup(prefab));
            }
        }

        public override void Disable()
        {
            base.Disable();
        }
    }
}
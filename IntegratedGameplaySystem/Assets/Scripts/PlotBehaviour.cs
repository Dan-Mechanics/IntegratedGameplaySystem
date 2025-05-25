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
        private readonly List<Plant> plants = new List<Plant>();
        
        public override void Start()
        {
            base.Start();

            PlantBlueprint blueprint = new PlantBlueprint.Builder()
                .SetName(name)
                .SetGrowOdds(7) // inspector exposed ??
                .SetMaterials(name)
                .SetItemSprite(name)
                .Build();

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);

                plants.Add(new Plant(blueprint, child));
                child.name = i.ToString();
            }
        }

        public override void Disable()
        {
            base.Disable();
            plants.ForEach(x => x.Dispose());
        }
    }
}
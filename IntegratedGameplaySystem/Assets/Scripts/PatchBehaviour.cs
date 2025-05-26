using UnityEngine;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// And this would then be where we have our like input handler classes and such. Component pattern perchange ??? !!
    /// </summary>
    [CreateAssetMenu(menuName = nameof(BaseBehaviour) + "/" + nameof(PatchBehaviour), fileName = "New " + nameof(PatchBehaviour))]
    public class PatchBehaviour : BaseBehaviour
    {
        public int count;
        public float dispersal;
        public GameObject plantPrefab;
        public PlayerContext playerContext;

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

            for (int i = 0; i < count; i++)
            {
                Transform plant = Instantiate(plantPrefab, GetRandomPos(), Quaternion.identity).transform;
                plant.name = $"{name}{InteractBehaviour.SPLITTER}{i}";
                // we can use the name for inforamtion.

                var newPlant = new Plant(blueprint, plant);
                newPlant.OnEarnMoney += playerContext.wallet.EarnMoney;
                plants.Add(newPlant);
                //child.name = i.ToString();
            }
        }

        /// <summary>
        /// Utls ??
        /// </summary>
        /// <returns></returns>
        private Vector3 GetRandomPos() 
        {
            Vector2 rand = Random.insideUnitCircle * dispersal;
            return new Vector3(rand.x, 0f, rand.y) + prefab.transform.position;
        }

        public override void Disable()
        {
            base.Disable();
            plants.ForEach(x => x.Dispose());
        }

        public Plant GetPlant(int index) => plants[index];
    }
}
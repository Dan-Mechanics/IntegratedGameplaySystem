using UnityEngine;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    public class PatchBehaviour : IStartable, IDisposable
    {
        public string name;
        public int count;
        public float dispersal;
        public GameObject plantPrefab;

        private readonly List<Plant> plants = new List<Plant>();
        
        public void Start()
        {
            PlantBlueprint blueprint = new PlantBlueprint.Builder()
                .SetName(name)
                .SetGrowOdds(7) // inspector exposed ??
                .SetMaterials(name)
                .SetItemSprite(name)
                .Build();

            for (int i = 0; i < count; i++)
            {
                Transform plant = Utils.SpawnPrefab(plantPrefab).transform;
                plant.position = GetRandomPos();

                var newPlant = new Plant(blueprint, plant);
                plants.Add(newPlant);
            }
        }

        /// <summary>
        /// UTILS ??
        /// </summary>
        private Vector3 GetRandomPos() 
        {
            Vector2 rand = Random.insideUnitCircle * dispersal;
            return new Vector3(rand.x, 0f, rand.y) + plantPrefab.transform.position;
        }

        public void Dispose() => plants.ForEach(x => x.Dispose());
        public Plant GetPlant(int index) => plants[index];
    }
}
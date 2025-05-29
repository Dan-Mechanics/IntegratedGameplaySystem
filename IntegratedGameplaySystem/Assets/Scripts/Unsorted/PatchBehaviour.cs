using UnityEngine;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    public class PatchBehaviour : IStartable, IDisposable
    {
        public const string PLANT_PREFAB_NAME = "plant";

        private readonly string name;
        private readonly int count;
        private readonly float dispersal;
        private readonly List<Plant> plants = new List<Plant>();

        public PatchBehaviour(string name, int count, float dispersal)
        {
            this.name = name;
            this.count = count;
            this.dispersal = dispersal;
        }

        public void Start()
        {
            IWorldService world = ServiceLocator<IWorldService>.Locate();
            IAssetService assets = ServiceLocator<IAssetService>.Locate();

            GameObject prefab = assets.GetByAgreedName(PLANT_PREFAB_NAME);

            PlantBlueprint blueprint = new PlantBlueprint.Builder()
                .SetName(name)
                .SetGrowOdds(7) // inspector exposed ??
                .SetMaterials(name)
                .SetItemSprite(name)
                .Build();

            for (int i = 0; i < count; i++)
            {
                GameObject go = Utils.SpawnPrefab(prefab);
                go.transform.position = GetRandomPos(prefab.transform.position);

                Plant plant = new Plant(blueprint, go.transform);
                plants.Add(plant);
                world.Add(go, plant);
            }
        }

        /// <summary>
        /// UTILS ??
        /// </summary>
        private Vector3 GetRandomPos(Vector3 offset) 
        {
            Vector2 rand = Random.insideUnitCircle * dispersal;
            return new Vector3(rand.x, 0f, rand.y) + offset;
        }

        public void Dispose() => plants.ForEach(x => x.Dispose());
        public Plant GetPlant(int index) => plants[index];
    }
}
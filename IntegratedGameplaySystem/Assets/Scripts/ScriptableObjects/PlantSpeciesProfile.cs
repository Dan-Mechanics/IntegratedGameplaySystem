using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// "Flyweight" ish.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(PlantSpeciesProfile), fileName = "New " + nameof(PlantSpeciesProfile))]
    public class PlantSpeciesProfile : ScriptableObject
    {
        public GameObject plantPrefab;
        public GameObject rainPrefab;
        [Min(1)] public int plantCount;
        [Min(0f)] public float dispersal;
        public int monetaryValue;
        [Tooltip("One in ...")]
        [Min(1)] public int growOdds;
        [Min(1)] public int wateredGrowOdds;
        public Sprite sprite;
        public Material[] materials;

        public void Populate(List<object> behaviours) 
        {
            for (int j = 0; j < plantCount; j++)
            {
                behaviours.Add(new Plant(this));
            }
        }
        
        private void OnValidate() 
        {
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = Resources.Load<Material>($"{name}/stage_{i + 1}_mat");
            }

            sprite = Resources.Load<Sprite>($"{name}/stage_{materials.Length}_img");
            Debug.Log(name.ToUpper());
        }
    }
}
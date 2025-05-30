using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// "Flyweight" ish.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(PlantSpeciesProfile), fileName = "New " + nameof(PlantSpeciesProfile))]
    public class PlantSpeciesProfile : ScriptableObject
    {
        [Min(1)] public int plantCount;
        [Min(0f)] public float dispersal;
        public int monetaryValue;
        [Tooltip("One in ...")]
        [Min(1)] public int growOdds;
        [Min(1)] public int wateredGrowOdds;
        public Sprite sprite;
        public Material[] materials;
        
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
using UnityEngine;
using System.Collections.Generic;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// "Flyweight"
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(PlantSpeciesProfile), fileName = "New " + nameof(PlantSpeciesProfile))]
    public class PlantSpeciesProfile : ScriptableObject
    {
        public int plantCount;
        public float dispersal;
        public int growOdds;
        public Sprite sprite;
        public Material[] materials;

        private void OnValidate() 
        {
            Debug.Log(name.ToUpper());
            
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = Resources.Load<Material>($"{name}/stage_{i + 1}_mat");
            }

            // or we say sample the last stage and make all the stuff sprites
            // idk if that will break the material tho ...
            //sprite = Resources.Load<Sprite>($"{name}/sprite");
            sprite = Resources.Load<Sprite>($"{name}/stage_{materials.Length}_img");
        }
    }
}
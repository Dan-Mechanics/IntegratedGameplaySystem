using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// "Flyweight" ish.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(PlantBlueprint), fileName = "New " + nameof(PlantBlueprint))]
    public class PlantBlueprint : ScriptableObject
    {
        public GameObject plantPrefab;
        public GameObject rainPrefab;
        public int monetaryValue;
        [Tooltip("One in ...")]
        [Min(1)] public int growOdds;
        [Tooltip("One in ...")]
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
            //Debug.Log(name.ToUpper());
        }
    }
}
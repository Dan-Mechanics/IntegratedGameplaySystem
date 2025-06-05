using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// "Flyweight" ish.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(PlantFlyweight), fileName = "New " + nameof(PlantFlyweight))]
    public class PlantFlyweight : ScriptableObject, IItemArchitype
    {
        public Sprite Sprite => sprite;
        public int MaxStackSize => maxCount;
        public int MonetaryValue => monetaryValue;

        public GameObject plantPrefab;
        public GameObject rainPrefab;
        public int monetaryValue;
        [Min(1), Tooltip("One in ...")] public int growOdds;
        [Min(1), Tooltip("One in ...")] public int wateredGrowOdds;
        [Min(1)] public int maxCount;

        public Sprite sprite;
        public Material[] materials;

        /// <summary>
        /// Does making the textures sprites mess
        /// with the performance ??
        /// </summary>
        private void OnValidate() 
        {
            sprite = Resources.Load<Sprite>($"{name}/stage_{materials.Length}_img");

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = Resources.Load<Material>($"{name}/stage_{i + 1}_mat");
            }
        }
    }
}
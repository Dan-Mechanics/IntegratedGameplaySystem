using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// "Flyweight" ish.
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(PlantBlueprint), fileName = "New " + nameof(PlantBlueprint))]
    public class PlantBlueprint : ScriptableObject, IItem
    {
        public Sprite Sprite => sprite;
        public int MaxCount => maxCount;
        public int Money => monetaryValue;
        public LayerMask Mask => mask;

        public GameObject plantPrefab;
        public GameObject rainPrefab;
        public int monetaryValue;
        [Min(1), Tooltip("One in ...")] public int growOdds;
        [Min(1), Tooltip("One in ...")] public int wateredGrowOdds;
        [Min(1)] public int maxCount;
        public LayerMask mask;

        public Sprite sprite;
        public Material[] materials;

        /// <summary>
        /// Does making the textures sprites mess
        /// with the performance ??
        /// </summary>
        private void OnValidate() 
        {
            sprite = Resources.Load<Sprite>($"{name}/stage_{materials.Length}_img");

            // Default + Plant.
            mask = 1 << 0 | 1 << LayerMask.NameToLayer(name);

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = Resources.Load<Material>($"{name}/stage_{i + 1}_mat");
            }
        }
    }
}
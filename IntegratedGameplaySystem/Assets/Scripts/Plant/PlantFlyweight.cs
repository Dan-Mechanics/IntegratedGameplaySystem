using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// "Flyweight" ish.
    /// Plant arhcittype ???
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/" + nameof(PlantFlyweight), fileName = "New " + nameof(PlantFlyweight))]
    public class PlantFlyweight : ScriptableObject, IItemArchitype
    {
        public Sprite Sprite => sprite;
        public Color ItemTint => itemTint;
        public int MonetaryValue => monetaryValue;

        [Range(0f, 100f)] public float dryGrowPercentage;
        [Range(0f, 100f)] public float wateredGrowGrowPercentage;
        [Min(1)] public int monetaryValue;
        public Color itemTint;

        public Material drySoil;
        public Material wetSoil;
        public GameObject plantPrefab;
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
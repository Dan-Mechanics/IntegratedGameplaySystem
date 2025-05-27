using System;
using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// For more than one plant.
    /// </summary>
    public class PlantBlueprint 
    {
        public string name;
        public int growOdds;
        public Material[] materials;
        public Sprite itemSprite;

        private void Setup() { }

        /// <summary>
        /// Builder pattern.
        /// </summary>
        public class Builder
        {
            private readonly PlantBlueprint blueprint;

            public Builder()
            {
                blueprint = new PlantBlueprint();
            }

            public Builder SetName(string name) 
            {
                blueprint.name = name;
                return this;
            }

            public Builder SetGrowOdds(int growOdds) 
            {
                blueprint.growOdds = growOdds;
                return this;
            }

            public Builder SetItemSprite(Sprite itemSprite) 
            {
                blueprint.itemSprite = itemSprite;
                return this;
            }

            /// <summary>
            /// Use resources.
            /// </summary>
            public Builder SetMaterials(Material[] materials) 
            {
                blueprint.materials = materials;
                return this;
            }

            /// <summary>
            /// BUG: dont have to add file extention ??
            /// </summary>
            public Builder SetMaterials(string name)
            {
                Material[] materials = new Material[5];
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = Resources.Load<Material>($"{name}/stage_{i + 1}_mat");
                }

                blueprint.materials = materials;
                return this;
            }

            public Builder SetItemSprite(string name)
            {
                blueprint.itemSprite = Resources.Load<Sprite>($"{name}/sprite.png");
                return this;
            }

            public PlantBlueprint Build()
            {
                blueprint.Setup();
                return blueprint;
            }
        }
    }
}

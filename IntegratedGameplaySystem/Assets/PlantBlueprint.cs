using System;
using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class PlantBlueprint 
    {
        public string name;
        public int growOdds;
        public Material[] materials;
        public Sprite itemSprite;

        public void Setup() { }

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

            public Builder SetMaterials(string name)
            {
                Material[] materials = new Material[5];
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = Resources.Load<Material>($"{name}/stage_{i + 1}_mat.mat");
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

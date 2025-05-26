using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectWitchGarden
{
    //[CreateAssetMenu(menuName = "Magical Plant", fileName = "New Magical Plant")]
    public class MagicalPlant : ScriptableObject
    {
        public enum Season { Spring, Summer, Fall, Winter }
        public enum Light { FullSun, HalfSun, FullMoon, HalfMoon }
        public enum Type { Plant, Flower, Bush, Mushroom }

        public new string name;
        public Material[] plantMaterials;

        public Season[] growingSeasons;
        public Light lightRequirement;
        public Type type;
        public float waterRequirementPerDay; // if this is 1, the player should water each day to avoid a dry plant.
                                             // if it is 0.5, once every 2 days.

        public Material GetPlantMaterial(int index)
        {
            if (plantMaterials.Length == 0) { Debug.LogError("Plant does not have materials."); }

            if (index < 0 || index > plantMaterials.Length - 1)
            {
                return plantMaterials[0]; // we know this will always exits because of the log error.
            }
            else
            {
                return plantMaterials[index];
            }
        }
    }
}
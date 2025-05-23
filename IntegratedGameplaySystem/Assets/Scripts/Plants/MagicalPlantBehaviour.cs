using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectWitchGarden
{
    public class MagicalPlantBehaviour : MonoBehaviour
    {
        [SerializeField] private MeshRenderer[] quads = null;
        [SerializeField] private MagicalPlant magicalPlant = null;

        private float ageingSpeed;
        private float age;
        private int visibleAge;

        private void Start()
        {
            gameObject.name = magicalPlant.name + "_magical_plant";
            SetMaterial(visibleAge);
            ChangeAgingSpeed();
        }

        private void ChangeAgingSpeed()
        {
            ageingSpeed = 1f / (Random.Range(5f, 30f) / magicalPlant.plantMaterials.Length);
        }

        private void Update()
        {
            age += Time.deltaTime * ageingSpeed;

            int roundedAge = (int)age;

            if (roundedAge != visibleAge)
            {
                SetMaterial(roundedAge);

                if (visibleAge > magicalPlant.plantMaterials.Length - 1) { age = 0f; ChangeAgingSpeed(); }
            }
        }

        private void SetMaterial(int index)
        {
            Material mat = magicalPlant.GetPlantMaterial(index);

            for (int i = 0; i < quads.Length; i++)
            {
                quads[i].material = mat;
            }

            visibleAge = index;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    public class Plot : IPlantSpawner, IInteractable, IDisposable, IHoverable
    {
        public string HoverTitle => $"Sprinkler upgrade for {settings.sprinklerPrice}";
        private readonly PlotSettings settings;
        private Plant[] plants;
        private bool isSprinkled;
        private readonly PlantFlyweight flyweight;
        private readonly Vector3 offset;

        public Plot(PlotSettings settings, PlantFlyweight flyweight, Vector3 offset)
        {
            this.settings = settings;
            this.flyweight = flyweight;
            this.offset = offset;
        }

        public void Interact()
        {
            isSprinkled = true;
            EventManager<int>.RaiseEvent(Occasion.LoseMoney, settings.sprinklerPrice);
        }

        public void Spawn(List<object> result)
        {
            GameObject button = Utils.SpawnPrefab(settings.buttonPrefab);
            button.transform.position += offset;

            ServiceLocator<IWorldService>.Locate().Add(button, this);

            plants = new Plant[settings.width * settings.width];

            for (int x = 0; x < settings.width; x++)
            {
                for (int z = 0; z < settings.width; z++)
                {
                    plants[x + (z * settings.width)] = new Plant(flyweight);
                    plants[x + (z * settings.width)].HasSprinkler += HasSprinkler;

                    plants[x + (z * settings.width)].transform.position += new Vector3(x * settings.spacing, 0f, z * settings.spacing) + offset;
                    // Utils.ApplyRandomRotation(temp.gameObject.transform);

                    result.Add(plants[x + (z * settings.width)]);
                }
            }

            result.Add(this);
        }

        public void Dispose() 
        {
            for (int i = 0; i < plants.Length; i++)
            {
                plants[i].HasSprinkler -= HasSprinkler;
            }

            plants = null;
        }

        private bool HasSprinkler() 
        {
            return isSprinkled;
        }
    }
}
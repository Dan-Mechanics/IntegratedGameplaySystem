using System.Collections.Generic;
using UnityEngine;

namespace IntegratedGameplaySystem
{
    /// <summary>
    /// Extract to plantupgrades so that we can have both dipseral and other hsit.
    /// </summary>
    public class Plot : IPlantSpawner, IDisposable
    {
        private readonly PermanentUpgrade sprinkler = new();
        private readonly RepeatableUpgrade grenade = new();
        
        private readonly PlotSettings settings;
        private readonly PlantFlyweight flyweight;
        private readonly Vector3 position;
        private readonly MoneyCentral money;

        private Plant[] plantsOnPlot;

        public Plot(PlotSettings settings, PlantFlyweight flyweight, int index, MoneyCentral money)
        {
            this.settings = settings;
            this.flyweight = flyweight;
            this.money = money;

            position = new Vector3(index * settings.width + settings.spacing / 2f, 0f, settings.spacing / 2f);
        }

        public void Spawn(List<object> result)
        {
            plantsOnPlot = new Plant[settings.width * settings.width];
            int index;

            for (int x = 0; x < settings.width; x++)
            {
                for (int z = 0; z < settings.width; z++)
                {
                    index = x + (z * settings.width);
                    var temp = new Plant(flyweight);

                    plantsOnPlot[index].transform.position += new Vector3(x * settings.spacing, 0f, z * settings.spacing) + position;
                    Utils.ApplyRandomRotation(plantsOnPlot[index].transform);

                    plantsOnPlot[index] = temp;
                    result.Add(plantsOnPlot[index]);
                }
            }

            result.Add(this);
        }

        public void Start() 
        {
            IWorldService world = ServiceLocator<IWorldService>.Locate();

            sprinkler.Setup(settings.sprinkler, world, position);
            sprinkler.OnCanAfford += money.CanAfford;

            grenade.Setup(settings.grenade, world, position);
            grenade.OnCanAfford += money.CanAfford;
            grenade.OnBuy += HarvestAll;

            for (int i = 0; i < plantsOnPlot.Length; i++)
            {
                plantsOnPlot[i].IsAlwaysWatered += sprinkler.GetHasBought;
            }
        }

        public void HarvestAll() 
        {
            for (int i = 0; i < plantsOnPlot.Length; i++)
            {
                plantsOnPlot[i].Interact();
            }
        }

        public void Dispose() 
        {
            for (int i = 0; i < plantsOnPlot.Length; i++)
            {
                plantsOnPlot[i].IsAlwaysWatered -= sprinkler.GetHasBought;
            }

            sprinkler.OnCanAfford -= money.CanAfford;

            grenade.OnCanAfford -= money.CanAfford;
            grenade.OnBuy -= HarvestAll;
        }
    }
}
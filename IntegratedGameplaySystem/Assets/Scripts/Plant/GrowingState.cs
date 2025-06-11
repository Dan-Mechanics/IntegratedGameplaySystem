namespace IntegratedGameplaySystem
{
    public class GrowingState : IPlantState
    {
        public PlantUnit Plant { get; set; }

        public string GetHoverTitle()
        {
            if (!Plant.IsWatered)
                return $"dry {Plant.flyweight.name}";

            return Plant.flyweight.name;
        }

        public void Tick() 
        {
            if (!Utils.RandomWithPercentage(Plant.IsWatered ? Plant.flyweight.wateredGrowGrowPercentage : Plant.flyweight.dryGrowPercentage))
                return;

            Plant.Grow();
        }

        public void Harvest() { }

        public void Interact()
        {
            Water();
        }

        public void Water()
        {
            Plant.SetWatered(true);
        }
    }
}
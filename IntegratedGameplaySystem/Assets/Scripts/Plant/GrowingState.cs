namespace IntegratedGameplaySystem
{
    public class GrowingState : PlantState
    {
        public override string GetHoverTitle()
        {
            if (!plant.IsWatered)
                return $"dry {plant.flyweight.name}";

            return plant.flyweight.name;
        }

        public override void Tick() 
        {
            if (!Utils.RandomWithPercentage(plant.IsWatered ? plant.flyweight.wateredGrowGrowPercentage : plant.flyweight.dryGrowPercentage))
                return;

            plant.Grow();
        }

        public override void Interact() => Water();
        public override void Water() => plant.SetWatered(true);
    }
}
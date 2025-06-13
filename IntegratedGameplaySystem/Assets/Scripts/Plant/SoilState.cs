namespace IntegratedGameplaySystem
{
    public class SoilState : PlantState
    {
        public override string GetHoverTitle() => $"barren {plant.flyweight.name} soil";

        public override void Interact()
        {
            plant.SetState(typeof(GrowingState));
            plant.RefreshMaterials();
        }

        public override void Water() => plant.SetWatered(true);
    }
}